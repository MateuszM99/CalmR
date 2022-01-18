using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity.Authentication;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly Token _token;
        private readonly HttpContext _httpContext;
        
        public AuthenticateService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<Token> tokenOptions,
            IEmailSender emailSender,
            IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
            _token = tokenOptions.Value;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<SignUpResponse> SignUp(SignUpRequest request, string ipAddress, string origin)
        {
            User user = new User
            {
                UserName = request.UserName,
                Email =  request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PsychologistId = request.PsychologistId,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException(string.Join(Environment.NewLine, result.Errors.Select(err => $"{err.Code}: {err.Description}")), "500");
            }

            if (request.PsychologistId != null)
            {
                var psychologistToUpdate = await _context.Psychologists.FindAsync(request.PsychologistId);
                psychologistToUpdate.UserId = user.Id;
                await _context.SaveChangesAsync(new CancellationToken());
            }

            await SendConfirmationEmail(user, origin);

            return new SignUpResponse()
            {
                Succeeded = true
            };
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request, string ipAddress)
        {
            User user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                throw new ApiException("Username or password was incorrect", "401");
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, true, false);
            
            if (!signInResult.Succeeded)
            {
                throw new ApiException("Username or password was incorrect", "401");
            }

            if (user.PsychologistId != null)
            {
                user.Psychologist = await _context.Psychologists.FirstOrDefaultAsync(p => p.UserId == user.Id);
            }
            
            var roles = (await _userManager.GetRolesAsync(user));
            string role = roles.Count != 0 ? roles[0] : null;
            string jwtToken = await GenerateJwtToken(user);

            await _userManager.UpdateAsync(user);

            return new AuthenticateResponse(user,
                role,
                jwtToken
                //""//refreshToken.Token
            );
        }
        
        public async Task ConfirmEmail(ConfirmEmailModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            { 
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }
            

            bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (isEmailConfirmed)
            {
                throw new ApiException("Email is already confirmed", StatusCodes.Status500InternalServerError.ToString());
            }

            var code = WebDecodeCode(model.Code);
            
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            { 
                throw new ApiException("Something went wrong", StatusCodes.Status500InternalServerError.ToString());
            }
        }

        public async Task ResendConfirmationEmail(string email,string origin)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            { 
                throw new ApiException("User not found", "404");
            }

            await SendConfirmationEmail(user, origin);
        }

        public async Task RequestPasswordReset(ResetPasswordRequest request, string ipAddress, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User not found", "404");
            }

            await SendPasswordResetEmail(user, origin);
        }
        
        public async Task ResetPassword(ResetPasswordModel model, string ipAddress, string origin)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new ApiException("User not found", "404");
            }

            var token = WebDecodeCode(model.Code);
            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            
            if (!result.Succeeded)
            {
                throw new ApiException(string.Join(Environment.NewLine, result.Errors.Select(err => $"{err.Code}: {err.Description}")), "500");
            }
        }

        private async Task SendConfirmationEmail(User user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncodeCode(code);
            var queryParams = new Dictionary<string, string>()
            {
                {"code", code},
                {"email", user.Email}
            };

            string callbackUrl = QueryHelpers.AddQueryString($"{origin}/account/confirm", queryParams);

            await _emailSender.SendEmailAsync(user.Email, "Account confirmation",
                $"Click this link to confirm account {callbackUrl}");
        }
        
        private async Task SendPasswordResetEmail(User user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncodeCode(code);
            var queryParams = new Dictionary<string, string>()
            {
                {"code", code},
                {"email", user.Email}
            };

            string callbackUrl = QueryHelpers.AddQueryString($"{origin}/account/reset", queryParams);

            await _emailSender.SendEmailAsync( user.Email, "Password reset",
                $"Click this link to reset password {callbackUrl}");
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var roles = (await _userManager.GetRolesAsync(user));
            string role = roles.Count != 0 ? roles[0] : null;
            byte[] secret = Encoding.ASCII.GetBytes(_token.Secret);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _token.Issuer,
                Audience = _token.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_token.Expiry),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        private string WebEncodeCode(string code) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        
        
        private string WebDecodeCode(string code) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
    }
}