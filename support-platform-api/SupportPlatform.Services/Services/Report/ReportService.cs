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

        public async Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsAsync(int reportId, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                if (user != null)
                {
                    string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                    ReportEntity report = await _repository.GetReportById(reportId);
                
                    if(report != null)
                    {
                        ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);

                        if(userRole == "Client" && report.UserId != userId)
                        {
                            return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Unauthorized);
                        }

                        return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
                    }
                }
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Unauthorized);
            }
            catch(Exception)
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

                    if (reportToCreate.File.Filename != null && reportToCreate.File.FileInBytes != null)
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
                Date = DateTime.Now,
                ModificationEntries = new List<ModificationEntryEntity>
                    {
                        new ModificationEntryEntity
                        {
                            Message = $"Klient {username} stworzył/a nowe zgłoszenie tytule: '{reportData.Heading}'",
                            Date = DateTime.Now
                        }
                    },
                Status = StatusEnum.New,
                UserId = userId
            };

            return report;
        }

        public async Task<IServiceResult<ReportListToReturnDto>> GetReportList(ReportListOptionsDto options, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                if(user != null)
                {
                    string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                    ICollection<ReportEntity> reports;

                    if (userRole == "Client")
                    {
                        reports = await _repository.GetReportsForClient(options.PageNumber, options.ItemsPerPage, options.ReportStatus, userId);
                    }
                    else
                    {
                        reports = await _repository.GetReportsForEmployee(options.PageNumber, options.ItemsPerPage, options.ReportStatus);
                    }


                    int totalPages = (int)Math.Ceiling((double)_repository.GetCount() / options.ItemsPerPage);
                    List<ReportListItemToReturnDto> reportItems = _reportMapper.Map(reports).ToList();

                    ReportListToReturnDto reportList = new ReportListToReturnDto
                    {
                        TotalPages = totalPages,
                        ReportListItems = reportItems
                    };

                    return new ServiceResult<ReportListToReturnDto>(ResultType.Correct, reportList);
                }

                return new ServiceResult<ReportListToReturnDto>(ResultType.Unauthorized);

            }
            catch(Exception e)
            {
                return new ServiceResult<ReportListToReturnDto> (ResultType.Error, new List<string> { "Błąd podczas pobierania listy zgłoszeń" });
            }
        }
    }
}
