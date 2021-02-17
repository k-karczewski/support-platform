using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportPlatform.Database
{
    public interface IReportRepository : IRepository<ReportEntity>
    {
        Task<ReportEntity> GetReportById(int id);
        Task<ICollection<ReportEntity>> GetAllReports();
    }
}
