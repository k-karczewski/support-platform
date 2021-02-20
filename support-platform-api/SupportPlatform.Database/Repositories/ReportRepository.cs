using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SupportPlatform.Database.Repositories
{
    public class ReportRepository : RepositoryBase<ReportEntity>, IReportRepository
    {
        protected override DbSet<ReportEntity> _dbSet => _dbContext.Reports;

        public ReportRepository(SupportPlatformDbContext dbContext) : base(dbContext) { }

        public async Task<ICollection<ReportEntity>> GetAllReports()
        {
            return await _dbSet.Include(r => r.Responses).Include(m => m.ModificationEntries).Include(a => a.Attachment).ToListAsync();
        }

        public async Task<ReportEntity> GetReportById(int id)
        {
            return await _dbSet.Include(r => r.Responses).Include(m => m.ModificationEntries).Include(a => a.Attachment).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
