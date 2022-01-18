using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Messages.Commands;
using Application.Messages.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using File = Domain.Entities.File;

namespace Application.Files.Commands.AddFile
{
    public class AddFileCommand : IRequest<long>
    {
        public IFormFile File { get; set; }
    }
    
    public class CommandHandler : IRequestHandler<AddFileCommand,long>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<long> Handle(AddFileCommand request, CancellationToken cancellationToken)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                
                var file = new File()
                {
                    Filename = request.File.FileName,
                    FileContent = ms.ToArray(),
                    FileExtension = request.File.ContentType
                };

                await _context.Files.AddAsync(file, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return file.Id;
            }
        }
    }
}