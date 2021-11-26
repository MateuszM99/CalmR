using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IUploadService
    {
        public Task<string> UploadPhotoAsync(IFormFile imageFile, int id);
    }
}