using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Application.Files.Queries.GetFileQuery
{
    public class GetFileQuery : IRequest<Domain.Entities.File>
    {
        public long FileId { get; set; }
    }

    public class CommandHandler : IRequestHandler<GetFileQuery, Domain.Entities.File>
    {
        private readonly IApplicationDbContext _context;

        public CommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<File> Handle(GetFileQuery request, CancellationToken cancellationToken)
        {
            return await _context.Files.FirstOrDefaultAsync(f => f.Id == request.FileId, cancellationToken);
        }
    }
}