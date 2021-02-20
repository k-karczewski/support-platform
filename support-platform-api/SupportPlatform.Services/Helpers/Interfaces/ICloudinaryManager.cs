using SupportPlatform.Database;

namespace SupportPlatform.Services
{
    public interface ICloudinaryManager
    {
        string UploadFile(FileToUploadDto fileToUpload, int userId);
    }
}
