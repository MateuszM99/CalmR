using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Appointments.Queries.GetAvailableHours
{
    public class GetAvailableHoursQuery : IRequest<List<DateTime>>
    {
        public int PsychologistId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int AppointmentDurationTime { get; set; }
    }

    public class CommandHandler : IRequestHandler<GetAvailableHoursQuery, List<DateTime>>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<DateTime>> Handle(GetAvailableHoursQuery request, CancellationToken cancellationToken)
        {
            const int workDayStartHour = 9;
            const int workDayHoursDuration = 8;

            var startOfWorkDayDate = new DateTime(request.AppointmentDate.Year, request.AppointmentDate.Month, request.AppointmentDate.Day, workDayStartHour, 0, 0);
            var endOfWorkDayDate = startOfWorkDayDate.AddHours(workDayHoursDuration);
            List<DateTime> listOfPossibleDates = getListOfPossibleDates(startOfWorkDayDate,endOfWorkDayDate);
            
            var listOfMadeAppointments = await _context.Appointments.Where(a =>
                                                                            a.Psychologist.Psychologist.Id == request.PsychologistId &&
                                                                            a.StartDate >= startOfWorkDayDate && a.StartDate <= endOfWorkDayDate)
                                                                            .ToListAsync(cancellationToken);
            
            listOfPossibleDates = listOfPossibleDates.Where(d => !listOfMadeAppointments.Any(a => d < a.StartDate.AddHours(a.DurationTime) && a.StartDate < d.AddHours(request.AppointmentDurationTime))).Where(d => d > DateTime.Now).ToList();


            return listOfPossibleDates;
        }

        private List<DateTime> getListOfPossibleDates(DateTime startOfWorkDayDate, DateTime endOfWorkDayDate)
        {
            List<DateTime> listOfPossibleDates = new List<DateTime>();
            DateTime dateToAdd = startOfWorkDayDate;
            while (dateToAdd <= endOfWorkDayDate)
            {
                listOfPossibleDates.Add(dateToAdd);
                dateToAdd = dateToAdd.AddMinutes(15);
            }

            return listOfPossibleDates;
        }
    }
}