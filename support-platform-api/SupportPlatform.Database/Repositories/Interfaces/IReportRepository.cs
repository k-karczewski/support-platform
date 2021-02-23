using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportPlatform.Database
{
    public interface IReportRepository : IRepository<ReportEntity>
    {
        int GetCount();
        Task<ReportEntity> GetReportById(int id);
        Task<ICollection<ReportEntity>> GetReportsForEmployee(int numberOfPage, int itemsPerPage, int status);
        Task<ICollection<ReportEntity>> GetReportsForClient(int numberOfPage, int itemsPerPage, int status, int userId);
    }
}
