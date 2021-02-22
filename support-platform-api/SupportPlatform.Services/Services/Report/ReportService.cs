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
        private readonly ICloudinaryManager _cloudinaryManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ReportEntityMapper _reportMapper;

        public ReportService(IReportRepository repository, UserManager<UserEntity> userManager, ICloudinaryManager cloudinaryManager, ReportEntityMapper reportEntity) 
        {
            _repository = repository;
            _userManager = userManager;
            _cloudinaryManager = cloudinaryManager;
            _reportMapper = reportEntity;
        }


        public async Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsForClientAsync(int reportId, int userId)
        {
            try
            {
                ReportEntity report = await _repository.GetReportById(reportId);
                    
                if(report.UserId == userId)
                {
                    ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);

                    return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
                }
                else
                {
                    return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Unauthorized);
                }
            }
            catch(Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas pobierania zgłoszenia" });
            }
        }

        public async Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsForEmployeeAsync(int reportId)
        {
            try
            {
                ReportEntity report = await _repository.GetReportById(reportId);

                ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);

                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
            }
            catch (Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas pobierania zgłoszenia" });
            }
        }


        public async Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(r => r.Reports).FirstOrDefaultAsync();

                if (user != null)
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

                    try 
                    {
                        bool result = _repository.AddNew(report);
                        ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);
                        
                        return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
                    }
                    catch(Exception)
                    {
                        string url = report.Attachment.Url;
                        await _cloudinaryManager.DeleteFileAsync(url.Substring(url.LastIndexOf('/')+1));

                        if(report.Id != 0)
                        {
                            _repository.Delete(report.Id);
                        }

                        throw;
                    }
                }
                else
                {
                    return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Unauthorized);
                }
            }
            catch(Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas tworzenia nowego zgłoszenia." });
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
