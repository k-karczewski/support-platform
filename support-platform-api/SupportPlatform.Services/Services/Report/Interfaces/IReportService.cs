using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Services
{
    public interface IReportService
    {
        Task<IServiceResult> CreateAsync(ReportToCreateDto reportToCreate, int userId);
    }
}
