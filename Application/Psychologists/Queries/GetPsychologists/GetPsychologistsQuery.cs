using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Appointments.Queries;
using Application.Common.DTO;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Psychologists.Queries
{
    public class GetPsychologistsQuery : IRequest<PagedResult<PsychologistDTO>>
    {
        public PageDTO pageDto { get; set; }
        public string textSearch { get; set; }
    }
    
    public class CommandHandler : PagingService<Psychologist>,IRequestHandler<GetPsychologistsQuery,PagedResult<PsychologistDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<PsychologistDTO>> Handle(GetPsychologistsQuery request, CancellationToken cancellationToken)
        {
            var psychologists = _context.Psychologists.Include(p => p.User).Include(a => a.Address).AsQueryable();

            psychologists = ApplyFilter(psychologists, request);
            psychologists = ApplyPaging(psychologists, request.pageDto);

            if (psychologists == null)
            {
                throw new ApiException("No psychologists found", StatusCodes.Status404NotFound.ToString());
            }
            
            var psychologistsList = await psychologists.ToListAsync(cancellationToken);
            
            var data = _mapper.Map<List<PsychologistDTO>>(psychologistsList);
            int dataCount = psychologistsList.Count();

            return new PagedResult<PsychologistDTO>(request.pageDto?.PageIndex,request.pageDto?.PageSize,dataCount, data);
        }

        private IQueryable<Psychologist> ApplyFilter(IQueryable<Psychologist> q, GetPsychologistsQuery request)
        {
            if (!String.IsNullOrWhiteSpace(request.textSearch))
            {
                q = q.Where(p => String.Concat(p.FirstName, p.LastName).Contains(request.textSearch));
            }

            return q;
        }
    }
}