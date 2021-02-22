using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public class CloudinaryManager : ICloudinaryManager
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryManager(IConfiguration configuration)
        {
            Account account = new Account
            {
                ApiKey = configuration.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = configuration.GetSection("CloudinarySettings:ApiSecret").Value,
                Cloud = configuration.GetSection("CloudinarySettings:CloudName").Value,
            };

            _cloudinary = new Cloudinary(account);
        }

        public async Task DeleteFileAsync(string publicId)
        {
            await _cloudinary.DeleteResourcesAsync(publicId);
        }

        public string UploadFile(FileToUploadDto fileToUpload, int userId)
        {
            Stream stream = new MemoryStream(fileToUpload.FileInBytes);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileToUpload.Filename, stream),
                PublicId = Guid.NewGuid().ToString(),
                Folder = $"reports/attachments/{userId}/",
                Overwrite = false       
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
