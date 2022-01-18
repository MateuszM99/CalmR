using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Application.Appointments.Queries;
using Application.Common.Interfaces;
using Application.Files.Commands.AddFile;
using Application.Files.Queries.GetFileQuery;
using Application.Messages.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalmR.Controllers
{
    public class FilesController : ApiControllerBase
    {
        [Route("addFile")]
        [HttpPost]
        public async Task<long> AddFile([FromForm]AddFileCommand command)
        {
            var response = await Mediator.Send(command);

            return response;
        }

        [Route("get/file")]
        [HttpGet]
        public async Task<FileResult> GetMessages([FromQuery]GetFileQuery query)
        {
            var response = await Mediator.Send(query);
            
            return File(response.FileContent, response.FileExtension, response.Filename);
        }
    }
}