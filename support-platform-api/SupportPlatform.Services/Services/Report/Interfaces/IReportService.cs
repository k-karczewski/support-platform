using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IReportService
    {
        /// <summary>
        /// Gets list of paginated reports. Optionally the list can be filtered by report status
        /// </summary>
        /// <param name="reportListParams">Pagination parameters of report list that will be returned (page number, page size, status filter).</param>
        /// <param name="userId">Id of user which made request</param>
        /// <returns>List of reports generated using parameters</returns>
        Task<IServiceResult<ReportListToReturnDto>> GetReportList(ReportListParams reportListParams, int userId);
        Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsAsync(int reportId, int userId);
        Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId);
        Task<IServiceResult<ReportStatusUpdateToReturnDto>> UpdateStatus(ReportStatusToUpdateDto statusToUpdate, int userId);
        Task<IServiceResult<ResponseToReturnDto>> SendResponse(ReportResponseToCreateDto responseToCreate, int userId);
    }
}
