using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Psychologists.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Psychologists.Commands.EditPsychologistProfile
{
    public class EditPsychologistProfileCommand : IRequest<PsychologistDTO>
    {
        public string PsychologistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile ProfileImage { get; set; }
        public int CostPerHour { get; set; }
        public string Description { get; set; }
       
        public string Country { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ZipCode { get; set; }
    }

    public class CommandHandler : IRequestHandler<EditPsychologistProfileCommand, PsychologistDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public CommandHandler(IApplicationDbContext context, IMapper mapper, IUploadService uploadService)
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<PsychologistDTO> Handle(EditPsychologistProfileCommand request, CancellationToken cancellationToken)
        {
            var psychologist = await _context.Psychologists.Include(u => u.User).Include(a => a.Address)
                .FirstOrDefaultAsync(x => x.UserId == request.PsychologistId, cancellationToken);
            
            if (psychologist == null)
            {
                throw new ApiException("No psychologist found", StatusCodes.Status404NotFound.ToString());
            }

            psychologist.FirstName = request.FirstName;
            psychologist.LastName = request.LastName;
            psychologist.CostPerHour = request.CostPerHour;
            psychologist.Description = request.Description;
            psychologist.Address.City = request.City;
            psychologist.Address.Country = request.Country;
            psychologist.Address.AddressLine1 = request.AddressLine1;
            psychologist.Address.AddressLine2 = request.AddressLine2;
            psychologist.Address.ZipCode = request.ZipCode;

            if (request.ProfileImage != null)
            {
                var profileImageUrl = await _uploadService.UploadPhotoAsync(request.ProfileImage, psychologist.Id);

                psychologist.ProfileImageUrl = profileImageUrl;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PsychologistDTO>(psychologist);
        }
    }
}