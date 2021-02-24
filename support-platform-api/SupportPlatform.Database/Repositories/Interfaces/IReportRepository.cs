using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Database
{
    public interface IReportRepository : IRepository<ReportEntity>
    {
        Task<ReportEntity> GetReportById(int id);
        IQueryable<ReportEntity> GetReports(string role, int userId = 0);
    }
}
