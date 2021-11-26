using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Options;
using AutoMapper.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class UploadService : IUploadService
    {
        private readonly CloudinaryOptions _cloudinaryOptions;

        public UploadService(IOptions<CloudinaryOptions> options)
        {
            _cloudinaryOptions = options.Value;
        }
        
        public async Task<string> UploadPhotoAsync(IFormFile imageFile, int id)
        {         
            var cloudinaryAccount = new Account(_cloudinaryOptions.CloudName, _cloudinaryOptions.ApiKey, _cloudinaryOptions.SecretKey);
        
            var cloudinary = new Cloudinary(cloudinaryAccount);
                   
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("profileImage",imageFile.OpenReadStream()),
                PublicId = $"CalmR/profileImages/{id}",
                Overwrite = true                
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            string url = uploadResult.Url.ToString();

            return url;
        }
    }
}