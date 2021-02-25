using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public class ReportService : IReportService
    {
        private readonly ICloudinaryManager _cloudinaryManager;
        private readonly IEmailSender _emailSender;
        private readonly IReportRepository _repository;
        private readonly ReportEntityMapper _reportMapper;
        private readonly UserManager<UserEntity> _userManager;

        public ReportService(IReportRepository repository, UserManager<UserEntity> userManager,
                            ICloudinaryManager cloudinaryManager, ReportEntityMapper reportEntity, IEmailSender emailSender) 
        {
            _cloudinaryManager = cloudinaryManager;
            _emailSender = emailSender;
            _repository = repository;
            _reportMapper = reportEntity;
            _userManager = userManager;
        }

        /// <summary>
        /// Creates and adds new report to database 
        /// </summary>
        /// <param name="reportToCreate">Report data from creation form</param>
        /// <param name="userId">Identifier of submitter</param>
        /// <returns>Service result with operation status and object to return</returns>
        public async Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(r => r.Reports).FirstOrDefaultAsync();

                ReportEntity report = CreateReport(reportToCreate, userId, user.UserName);

                AttachmentEntity attachment = CreateAttachment(reportToCreate.File, userId);
                report.Attachment = attachment;

                try
                {
                    _repository.AddNew(report);
                }
                catch (Exception)
                {
                    if (report.Attachment != null)
                    {
                        string url = report.Attachment.Url;
                        await _cloudinaryManager.DeleteFileAsync(url.Substring(url.LastIndexOf('/') + 1));
                    }

                    throw;
                }

                ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);

                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
            }
            catch (Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas tworzenia nowego zgłoszenia." });
            }
        }

        /// <summary>
        /// Gets detailed version of report by id
        /// </summary>
        /// <param name="reportId">Identifier of report</param>
        /// <param name="userId">Identifier of user</param>
        /// <returns>Service result with operation status and object to return</returns>
        public async Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsAsync(int reportId, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                ReportEntity report = await _repository.GetReportById(reportId, userRole, userId);
                
                if(report != null)
                {
                    ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(report);

                    return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
                }
               
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Unauthorized);
            }
            catch(Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas pobierania zgłoszenia" });
            }
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

        /// <summary>
        /// Adds new response for report
        /// </summary>
        /// <param name="responseToCreate">Data of new response</param>
        /// <param name="userId">Identifier of currently authorized user</param>
        /// <returns>Service result with operation status and object to return (Updated details of the report)</returns>
        public async Task<IServiceResult<ReportDetailsToReturnDto>> SendResponse(ReportResponseToCreateDto responseToCreate, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                ReportEntity reportToUpdate = await _repository.GetReportById(responseToCreate.ReportId, userRole, userId);

                if (reportToUpdate != null)
                {
                    ResponseEntity response = new ResponseEntity
                    {
                        Date = DateTime.Now,
                        Message = responseToCreate.Message,
                        UserId = userId
                    };

                    reportToUpdate.Responses.Add(response);

                    reportToUpdate.ModificationEntries.Add(HistoryEntriesGenerator.GetNewResponseEntry(user.UserName, reportToUpdate.Heading));

                    _repository.SaveChanges();

                    await SendEmployeeRespondedEmail(reportToUpdate.UserId, reportToUpdate.Heading);

                    ReportDetailsToReturnDto reportToReturn = _reportMapper.Map(reportToUpdate);

                    return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Correct, reportToReturn);
                }

                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Failed, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }
            catch (Exception)
            {
                return new ServiceResult<ReportDetailsToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }
        }

        /// <summary>
        /// Updates status of report
        /// </summary>
        /// <param name="statusToUpdate">New status and report identifier</param>
        /// <param name="userId">Identifier of currently authorized user</param>
        /// <returns>Service result with operation status and object to return (Updated modifiation entries and status)</returns>
        public async Task<IServiceResult<ReportStatusUpdateToReturnDto>> UpdateStatus(ReportStatusToUpdateDto statusToUpdate, int userId)
        {
            try
            {
                UserEntity user = await _userManager.Users.Where(x => x.Id == userId).Include(ur => ur.UserRoles).ThenInclude(r => r.Role).FirstOrDefaultAsync();

                string userRole = user.UserRoles.Select(x => x.Role).FirstOrDefault().Name;

                ReportEntity reportToUpdate = await _repository.GetReportById(statusToUpdate.ReportId, userRole, userId);

                if(reportToUpdate != null)
                {
                    reportToUpdate.Status = (StatusEnum)Enum.Parse(typeof(StatusEnum), statusToUpdate.NewStatus.ToString());

                    string statusName = ConvertToStatusName(reportToUpdate.Status);

                    reportToUpdate.ModificationEntries.Add(HistoryEntriesGenerator.GetStatusUpdatedEntry(user.UserName, statusName));

                    _repository.SaveChanges();

                    await SendStatusChangedEmail(reportToUpdate.UserId, reportToUpdate.Heading, reportToUpdate.Status);

                    ReportStatusUpdateToReturnDto statusUpdated = new ReportStatusUpdateToReturnDto
                    {
                        ModificationEntries = _reportMapper.Map(reportToUpdate.ModificationEntries).ToList(),
                        Status = statusToUpdate.NewStatus
                    };

                    return new ServiceResult<ReportStatusUpdateToReturnDto>(ResultType.Correct, statusUpdated);
                }
           
                return new ServiceResult<ReportStatusUpdateToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }
            catch(Exception)
            {
                return new ServiceResult<ReportStatusUpdateToReturnDto>(ResultType.Error, new List<string> { "Błąd podczas aktualizacji zgłoszenia" });
            }
        }

        /// <summary>
        /// Converts status enum to string value
        /// </summary>
        /// <param name="status">Report status enum</param>
        /// <returns>String value of parameter</returns>
        private string ConvertToStatusName(StatusEnum status)
        {
            switch(status)
            {
                case StatusEnum.New: return "Nowe";
                case StatusEnum.Pending: return "W trakcie rozpatrywania";
                default: return "Zakończone";
            }
        }

        /// <summary>
        /// Creates attachment if file has been received. Returns null otherwise.
        /// </summary>
        /// <param name="file">Attachment data</param>
        /// <param name="userId">Identifier of submitter</param>
        /// <returns>Generated attachment entity or null</returns>
        private AttachmentEntity CreateAttachment(FileToUploadDto file, int userId)
        {
            if (file.Filename != null && file.FileInBytes != null)
            {
                string attachmentUrl = _cloudinaryManager.UploadFile(file, userId);

                AttachmentEntity attachment = new AttachmentEntity
                {
                    Name = file.Filename,
                    Url = attachmentUrl
                };

                return attachment;
            }

            return null;
        }

        /// <summary>
        /// Created new Report Entity object based on ReportToCreateDto
        /// </summary>
        /// <param name="reportData">Data for new report</param>
        /// <param name="userId">Identifier of sumbitter</param>
        /// <param name="username">Username of submitter</param>
        /// <returns>Created instance of ReportEntity</returns>
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
                            Message = $"Klient {username} stworzył(a) nowe zgłoszenie o tytule: '{reportData.Heading}'",
                            Date = DateTime.Now
                        }
                    },
                Status = StatusEnum.New,
                UserId = userId
            };

            return report;
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

        /// <summary>
        /// Sends email with status changed information
        /// </summary>
        /// <param name="recipentId">Identifier of recipent</param>
        /// <param name="reportHeading">Heading of report</param>
        /// <param name="statusEnum">New status of report</param>
        private async Task SendStatusChangedEmail(int recipentId, string reportHeading, StatusEnum statusEnum)
        {
            UserEntity recipent = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == recipentId);
            string status = ConvertToStatusName(statusEnum);
            await _emailSender.SendStatusChanged(recipent.UserName, recipent.Email, reportHeading, status);
        }

        /// <summary>
        /// Sends email with information about new response
        /// </summary>
        /// <param name="recipentId">Identifier of recipent</param>
        /// <param name="reportHeading">Heading of report</param>
        private async Task SendEmployeeRespondedEmail(int recipentId, string reportHeading)
        {
            UserEntity recipent = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == recipentId);
            await _emailSender.SendEmployeeResponded(recipent.UserName, recipent.Email, reportHeading);
        }
    }
}
