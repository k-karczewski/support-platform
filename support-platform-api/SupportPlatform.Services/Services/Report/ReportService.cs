using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ICloudinaryManager _cloudinaryManager;

        public ReportService(IReportRepository repository, UserManager<UserEntity> userManager, ICloudinaryManager cloudinaryManager) 
        {
            _repository = repository;
            _userManager = userManager;
            _cloudinaryManager = cloudinaryManager;
        }

        public async Task<IServiceResult> CreateAsync(ReportToCreateDto reportToCreate, int userId)
        {
            UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(r => r.Reports).FirstOrDefaultAsync();

            if(user != null)
            {
                ReportEntity report = CreateReport(reportToCreate, userId, user.UserName);

                if (reportToCreate.File != null)
                {
                    string attachmentUrl = _cloudinaryManager.UploadFile(reportToCreate.File, userId);
                    
                    AttachmentEntity attachment = new AttachmentEntity
                    {
                        Name = reportToCreate.File.Filename,
                        Url = attachmentUrl
                    };

                    report.Attachment = attachment;
                }

                bool result = _repository.AddNew(report);

                return new ServiceResult(ResultType.Correct);
            }
            else
            {
                return new ServiceResult(ResultType.Unauthorized);
            }
        }

        private ReportEntity CreateReport(ReportToCreateDto reportData, int userId, string username)
        {
            ReportEntity report = new ReportEntity
            {
                Heading = reportData.Heading,
                Message = reportData.Message,
                ModificationEntries = new List<ModificationEntryEntity>
                    {
                        new ModificationEntryEntity
                        {
                            Message = $"Klient {username} stworzył/a nowe zgłoszenie tytule: '{reportData.Heading}'"
                        }
                    },
                Status = StatusEnum.New,
                UserId = userId
            };

            return report;
        }
    }
}
