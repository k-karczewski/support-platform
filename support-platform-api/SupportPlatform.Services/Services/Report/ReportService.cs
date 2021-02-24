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

        /// <summary>
        /// Gets list of paginated reports. Optionally the list can be filtered by report status
        /// </summary>
        /// <param name="reportListParams">Pagination parameters of report list that will be returned (page number, page size, status filter).</param>
        /// <param name="userId">Id of user which made request</param>
        /// <returns>List of reports generated using parameters</returns>
        public async Task<IServiceResult<ReportListToReturnDto>> GetReportList(ReportListParams reportListParams, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                if(user != null)
                {
                    string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                    IQueryable<ReportEntity> reports = _repository.GetReports(userRole, userId);

                    reports = PaginateCollection(reports, reportListParams);

                    int totalPages = (int)Math.Ceiling(reports.Count() / (double)reportListParams.PageSize);

                    List<ReportListItemToReturnDto> reportItems = _reportMapper.Map(await reports.ToListAsync()).ToList();

                    ReportListToReturnDto reportList = new ReportListToReturnDto
                    {
                        TotalPages = totalPages,
                        ReportListItems = reportItems
                    };

                    return new ServiceResult<ReportListToReturnDto>(ResultType.Correct, reportList);
                }

                return new ServiceResult<ReportListToReturnDto>(ResultType.Unauthorized);

            }
            catch(Exception)
            {
                return new ServiceResult<ReportListToReturnDto> (ResultType.Error, new List<string> { "Błąd podczas pobierania listy zgłoszeń" });
            }
        }


        public async Task<IServiceResult<ReportStatusUpdateToReturnDto>> UpdateStatus(ReportStatusToUpdateDto statusToUpdate, int userId)
        {
            UserEntity user = await _userManager.FindByIdAsync(userId.ToString());

            if(user != null)
            {
                ReportEntity reportToUpdate = await _repository.GetReportById(statusToUpdate.ReportId);

                if(reportToUpdate != null)
                {
                    reportToUpdate.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), statusToUpdate.NewStatus.ToString());
                    string statusName = ConvertToStatusName(reportToUpdate.Status);

                    reportToUpdate.ModificationEntries.Add(new ModificationEntryEntity
                    {
                        Date = DateTime.Now,
                        Message = $"Pracownik {user.UserName} zmienił status zgłoszenia na '{statusName}'"
                    });

                    _repository.SaveChanges();


                    ReportStatusUpdateToReturnDto statusUpdated = new ReportStatusUpdateToReturnDto
                    {
                        ModificationEntries = _reportMapper.Map(reportToUpdate.ModificationEntries).ToList(),
                        Status = (int)reportToUpdate.Status
                    };

                    return new ServiceResult<ReportStatusUpdateToReturnDto>(ResultType.Correct, statusUpdated);
                }
            }
            return new ServiceResult<ReportStatusUpdateToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
        }

        private string ConvertToStatusName(StatusEnum status)
        {
            switch(status)
            {
                case StatusEnum.New: return "Nowe";
                case StatusEnum.Pending: return "W trakcie rozpatrywania";
                default: return "Zakończone";
            }
        }

        public async Task<IServiceResult<ResponseToReturnDto>> SendResponse(ReportResponseToCreateDto responseToCreate, int userId)
        {
            try
            {
                UserEntity user = await _userManager.FindByIdAsync(userId.ToString());

                if (user != null)
                {
                    ReportEntity reportToUpdate = await _repository.GetReportById(responseToCreate.ReportId);

                    if (reportToUpdate != null)
                    {
                        ResponseEntity response = new ResponseEntity
                        {
                            Date = DateTime.Now,
                            Message = responseToCreate.Message,
                            UserId = userId
                        };

                        reportToUpdate.Responses.Add(response);

                        reportToUpdate.ModificationEntries.Add(new ModificationEntryEntity
                        {
                            Date = DateTime.Now,
                            Message = $"Pracownik {user.UserName} odpowiedział na zgłoszenie ${reportToUpdate.Heading}"
                        });

                        _repository.SaveChanges();


                        ResponseToReturnDto responseToReturn = _reportMapper.Map(response);
                        responseToReturn.ModificationEntry = new ModificationEntryToReturnDto
                        {
                            Date = DateTime.Now.ToString(),
                            Message = $"Pracownik {user.UserName} odpowiedział na zgłoszenie ${reportToUpdate.Heading}"
                        };

                        return new ServiceResult<ResponseToReturnDto>(ResultType.Correct, responseToReturn);
                    }
                }

                return new ServiceResult<ResponseToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }
            catch(Exception e)
            {
                return new ServiceResult<ResponseToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }

        }

        /// <summary>
        /// Creates paginated (filtered out) collection based on pagination parameters
        /// </summary>
        /// <param name="source">Source data collection</param>
        /// <param name="paginationParameters">Parameters based on which the collection will be generated</param>
        /// <returns>Paginated source collection by parameters</returns>
        private IQueryable<ReportEntity> PaginateCollection(IQueryable<ReportEntity> source, ReportListParams paginationParameters)
        {
            
            if(paginationParameters.StatusFilter != null)
            {
                source = source.Where(s => (int)s.Status == paginationParameters.StatusFilter);
            }

            return source
                        .OrderByDescending(d => d.Date.Date)
                        .ThenByDescending(d => d.Date.TimeOfDay)
                        .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                        .Take(paginationParameters.PageSize);

        }
    }
}
