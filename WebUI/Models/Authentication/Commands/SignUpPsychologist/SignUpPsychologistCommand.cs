using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity.Authentication;
using Infrastructure.Identity.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CalmR.Models.Authentication.Commands.SignUpPsychologist
{
    public class SignUpPsychologistCommand : SignUpRequest,IRequest<SignUpResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<SignUpPsychologistCommand,SignUpResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IAuthenticateService _authenticateService;
        private readonly HttpContext _httpContext;
        private readonly IUploadService _uploadService;

        public CommandHandler(IApplicationDbContext context, IAuthenticateService authenticateService, IHttpContextAccessor httpContextAccessor, IUploadService uploadService)
        {
            _context = context;
            _authenticateService = authenticateService;
            _uploadService = uploadService;
            this._httpContext = (httpContextAccessor != null) ? httpContextAccessor.HttpContext : throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<SignUpResponse> Handle(SignUpPsychologistCommand request, CancellationToken cancellationToken)
        {
            const string defaultProfileImageUrl = "https://www.personality-insights.com/wp-content/uploads/2017/12/default-profile-pic-e1513291410505.jpg";
            
            Psychologist newPsychologist = new Psychologist()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProfileImageUrl = defaultProfileImageUrl,
                Address = new Address()
                {
                    Country = request.Country,
                    City = request.City,
                    AddressLine1 = request.AddressLine1,
                    AddressLine2 = request.AddressLine2,
                    ZipCode = request.ZipCode
                }
            };

            await _context.Psychologists.AddAsync(newPsychologist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var signUpRequest = new SignUpRequest()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                PsychologistId = newPsychologist.Id
            };
            
            SignUpResponse signUpResponse = await _authenticateService.SignUp(signUpRequest, RequestHelpers.GetIpAddress(_httpContext), RequestHelpers.GetOrigin(_httpContext));
            if (!signUpResponse.Succeeded)
            {
                _context.Psychologists.Remove(newPsychologist);
                throw new ApiException("Something went wrong",StatusCodes.Status500InternalServerError.ToString());
            }

            if (request.ProfileImage != null)
            {
                var profileImageUrl = await _uploadService.UploadPhotoAsync(request.ProfileImage, newPsychologist.Id);

                newPsychologist.ProfileImageUrl = profileImageUrl;
            }

            return signUpResponse;
        }
    }
}