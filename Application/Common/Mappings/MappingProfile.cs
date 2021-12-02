using System.Linq;
using Application.Appointments.Queries;
using Application.Conversations.Queries;
using Application.Messages.Queries;
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
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.Address));

            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName));
            //.ForMember(dest => dest.SenderImageUrl, opt => opt.MapFrom(src => src.Sender.))

            CreateMap<Conversation, ConversationDTO>()
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.Messages.FirstOrDefault()));
        }
    }
}