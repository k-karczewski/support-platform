using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Creates and adds new report to database 
        /// </summary>
        /// <param name="reportToCreate">Report data from creation form</param>
        /// <param name="userId">Identifier of submitter</param>
        /// <returns>Service result with operation status and object to return</returns>
        Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId);

        /// <summary>
        /// Gets detailed version of report by id
        /// </summary>
        /// <param name="reportId">Identifier of report</param>
        /// <param name="userId">Identifier of user</param>
        /// <returns>Service result with operation status and object to return</returns>
        Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsAsync(int reportId, int userId);

        /// <summary>
        /// Gets list of paginated reports. Optionally the list can be filtered by report status
        /// </summary>
        /// <param name="reportListParams">Pagination parameters of report list that will be returned (page number, page size, status filter).</param>
        /// <param name="userId">Id of user which made request</param>
        /// <returns>List of reports generated using parameters</returns>
        Task<IServiceResult<ReportListToReturnDto>> GetReportList(ReportListParams reportListParams, int userId);

        /// <summary>
        /// Adds new response for report
        /// </summary>
        /// <param name="responseToCreate">Data of new response</param>
        /// <param name="userId">Identifier of currently authorized user</param>
        /// <returns>Service result with operation status and object to return (Updated details of the report)</returns>
        Task<IServiceResult<ReportDetailsToReturnDto>> SendResponse(ReportResponseToCreateDto responseToCreate, int userId);
        
        /// <summary>
        /// Updates status of report
        /// </summary>
        /// <param name="statusToUpdate">New status and report identifier</param>
        /// <param name="userId">Identifier of currently authorized user</param>
        /// <returns>Service result with operation status and object to return (Updated modifiation entries and status)</returns>
        Task<IServiceResult<ReportStatusUpdateToReturnDto>> UpdateStatus(ReportStatusToUpdateDto statusToUpdate, int userId);
    }
}
