using System.Linq;
using Application.Appointments.Queries;
using Application.Common.DTO;
using Application.Common.Interfaces;
using Application.Conversations.Queries;
using Application.Messages.Queries;
using Application.Psychologists.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        private readonly ICurrentUserService _currentUserService;
        public MappingProfile(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.Psychologist, opt => opt.MapFrom(src => src.Psychologist.Psychologist))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Client));

            CreateMap<Psychologist, PsychologistDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Address.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Address.AddressLine2))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode));

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
            
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.SentByMe,
                    opt => opt.MapFrom(src => src.Sender.Id == _currentUserService.UserId))
                .ForMember(dest => dest.ConversationId, opt => opt.MapFrom(src => src.ConversationId))
                .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.FileId))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File != null ? src.File.Filename : ""));
            //.ForMember(dest => dest.SenderImageUrl, opt => opt.MapFrom(src => src.Sender.))

            CreateMap<Participant, ConversationUserDTO>()
                .ForMember(dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.User.Psychologist != null ? src.User.Psychologist.FirstName : ""))
                .ForMember(dest => dest.LastName,
                    opt => opt.MapFrom(src => src.User.Psychologist != null ? src.User.Psychologist.LastName : ""))
                .ForMember(dest => dest.ProfileImageUrl,
                    opt => opt.MapFrom(
                        src => src.User.Psychologist != null ? src.User.Psychologist.ProfileImageUrl : ""));
            
            CreateMap<Conversation, ConversationDTO>()
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault()))
                .ForMember(dest => dest.ConversationParticipants,
                    opt => opt.MapFrom(src => src.Participants.Where(u => u.UserId != _currentUserService.UserId)));
        }
    }
}