using SupportPlatform.Database;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface ICloudinaryManager
    {
        /// <summary>
        /// Deletes file from Cloudinary
        /// </summary>
        /// <param name="publicId">Public identifier of resource</param>
        Task DeleteFileAsync(string publicId);

        /// <summary>
        /// Uploads file to Cloudinary
        /// </summary>
        /// <param name="fileToUpload">File to be uploaded</param>
        /// <param name="userId">Identifier of sumbitter</param>
        /// <returns>Absolute uri to uploaded resource</returns>
        string UploadFile(FileToUploadDto fileToUpload, int userId);
    }
}
