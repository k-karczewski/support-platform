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

        public IQueryable<ReportEntity> GetReports(string role, int userId = 0)
        {
            switch(role)
            {
                case RoleNames.Client:
                    return _dbSet.Where(u => u.UserId == userId).Include(u => u.User);
                default:
                    return _dbSet.Include(u => u.User);
            }
        }


        public async Task<ReportEntity> GetReportById(int id)
        {
            return await _dbSet
                            .Include(r => r.Responses)
                            .Include(m => m.ModificationEntries)
                            .Include(a => a.Attachment)
                            .Include(u => u.User)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
