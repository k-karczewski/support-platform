using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IReportService
    {
        Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsAsync(int reportId, int userId);
        Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId);
        Task<IServiceResult<ReportListToReturnDto>> GetReportList(ReportListOptionsDto options, int userId);
    }
}
