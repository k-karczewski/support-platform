using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Database.Repositories
{
    public class ReportRepository : RepositoryBase<ReportEntity>, IReportRepository
    {
        protected override DbSet<ReportEntity> _dbSet => _dbContext.Reports;

        public ReportRepository(SupportPlatformDbContext dbContext) : base(dbContext) { }


        public int GetCount()
        {
            return _dbSet.Count();
        }

        public async Task<ICollection<ReportEntity>> GetReportsForClient(int numberOfPage, int itemsPerPage, int status, int userId)
        {
            return await _dbSet
                            .Where(u => u.UserId == userId)
                            .Where(s => (int)s.Status == status)
                            .OrderByDescending(d => d.Date.Date)
                            .Skip(numberOfPage * itemsPerPage)
                            .Take(itemsPerPage)
                            .ToListAsync();
        }

        public async Task<ICollection<ReportEntity>> GetReportsForEmployee(int numberOfPage, int itemsPerPage, int status)
        {
            return await _dbSet
                            .Where(s => (int)s.Status == status)
                            .OrderByDescending(d => d.Date.Date)
                            .ThenByDescending(d => d.Date.TimeOfDay)
                            .Skip(numberOfPage * itemsPerPage)
                            .Take(itemsPerPage)
                            .Include(u => u.User)
                            .ToListAsync();
        }

        public async Task<ReportEntity> GetReportById(int id)
        {
            return await _dbSet.Include(r => r.Responses).Include(m => m.ModificationEntries).Include(a => a.Attachment).Include(u => u.User).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
