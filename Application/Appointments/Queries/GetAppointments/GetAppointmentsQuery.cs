using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTO;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Queries
{
    public class GetUserAppointmentsQuery : IRequest<PagedResult<AppointmentDTO>>
    {
        public PageDTO pageDto { get; set; }
        public string TextSearch { get; set; }
        public string AppointmentStatus { get; set; }
        public string SortType { get; set; }
        public DateTime? DateFilter { get; set; }
    }
    
    public class CommandHandler : PagingService<Appointment>,IRequestHandler<GetUserAppointmentsQuery,PagedResult<AppointmentDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<AppointmentDTO>> Handle(GetUserAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_currentUser.UserId);

            if (user == null)
            {
                throw new ApiException("User not found", StatusCodes.Status404NotFound.ToString());
            }

            var appointments = _context.Appointments
                                                            .Where(a => a.ClientId == user.Id || a.PsychologistId == user.Id)
                                                            .Include(a => a.Psychologist)
                                                            .ThenInclude(u => u.Psychologist)
                                                            .ThenInclude(a => a.Address)
                                                            .Include(a => a.Client)
                                                            .AsQueryable();
            
            appointments = ApplyFilter(appointments, request);
            appointments = ApplyPaging(appointments, request.pageDto);

            if (appointments == null)
            {
                throw new ApiException("No appointments found", StatusCodes.Status404NotFound.ToString());
            }

            var appointmentsList = await appointments.ToListAsync(cancellationToken);

            var data = _mapper.Map<List<AppointmentDTO>>(appointmentsList);
            var dataCount = appointmentsList.Count();
            
            return new PagedResult<AppointmentDTO>(request.pageDto?.PageIndex,request.pageDto?.PageSize,dataCount, data);
        }

        private IQueryable<Appointment> ApplyFilter(IQueryable<Appointment> data, GetUserAppointmentsQuery query)
        {
            if (!String.IsNullOrWhiteSpace(query.TextSearch))
            {
                data = data.Where(a =>
                    String.Concat(a.Psychologist.Psychologist.FirstName, a.Psychologist.Psychologist.LastName)
                        .Contains(query.TextSearch));
            }

            if (!String.IsNullOrWhiteSpace(query.AppointmentStatus))
            {
                if (query.AppointmentStatus == "awaiting")
                {
                    data = data.Where(a => a.Status == AppointmentStatus.AwaitingForConfirmation);
                }

                if (query.AppointmentStatus == "confirmed")
                {
                    data = data.Where(a => a.Status == AppointmentStatus.Confirmed);
                }
                
                if (query.AppointmentStatus == "rejected")
                {
                    data = data.Where(a => a.Status == AppointmentStatus.Rejected);
                }
                
                if (query.AppointmentStatus == "ended")
                {
                    data = data.Where(a => a.Status == AppointmentStatus.Ended);
                }
                
                if (query.AppointmentStatus == "cancelled")
                {
                    data = data.Where(a => a.Status == AppointmentStatus.Cancelled);
                }
            }

            if (query.DateFilter != null)
            {
                data = data.Where(a => a.StartDate.Date == query.DateFilter.Value.Date);
            }
            
            if (!String.IsNullOrWhiteSpace(query.SortType))
            {
                if (query.SortType == "date")
                {
                    data = data.OrderByDescending(a => a.StartDate);
                }
            }

            return data;
        }
    }
}