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
        public Speciality Speciality { get; set; }
        public int CostPerHour { get; set; }
        public string Description { get; set; }
        
        public IFormFile ProfileImage { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
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
            Psychologist newPsychologist = new Psychologist()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Speciality = request.Speciality,
                CostPerHour = request.CostPerHour,
                Description = request.Description,
                Address = new Address()
                {
                    Country = request.Country,
                    City = request.City,
                    Street = request.Street,
                    HouseNumber = request.HouseNumber,
                    ApartmentNumber = request.ApartmentNumber,
                    ZipCode = request.ZipCode
                }
            };

            await _context.Psychologists.AddAsync(newPsychologist, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            string profileImageUrl = await _uploadService.UploadPhotoAsync(request.ProfileImage, newPsychologist.Id);
            newPsychologist.ProfileImageUrl = profileImageUrl;
            await _context.SaveChangesAsync(cancellationToken);

            var signUpRequest = new SignUpRequest()
            {
                UserName = request.UserName,
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword
            };
            
            SignUpResponse signUpResponse = await _authenticateService.SignUp(signUpRequest, RequestHelpers.GetIpAddress(_httpContext), RequestHelpers.GetOrigin(_httpContext));
            if (!signUpResponse.Succeeded)
            {
                throw new ApiException("Something went wrong",StatusCodes.Status500InternalServerError.ToString());
            }

            return signUpResponse;
        }
    }
}