using SupportPlatform.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IReportService
    {
        Task<IServiceResult<ReportDetailsToReturnDto>> GetReportDetailsForClientAsync(int reportId, int userId);
        Task<IServiceResult<ReportDetailsToReturnDto>> CreateAsync(ReportToCreateDto reportToCreate, int userId);
    }
}
