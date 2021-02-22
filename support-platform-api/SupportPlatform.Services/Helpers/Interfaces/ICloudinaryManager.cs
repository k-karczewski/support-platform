using SupportPlatform.Database;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface ICloudinaryManager
    {
        string UploadFile(FileToUploadDto fileToUpload, int userId);
        Task DeleteFileAsync(string publicId);
    }
}
