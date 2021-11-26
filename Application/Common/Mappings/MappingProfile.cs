using Application.Appointments.Queries;
using Application.Psychologists.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.Psychologist, opt => opt.MapFrom(src => src.Psychologist.Psychologist));

            CreateMap<Psychologist, PsychologistDTO>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.Address));
        }
    }
}