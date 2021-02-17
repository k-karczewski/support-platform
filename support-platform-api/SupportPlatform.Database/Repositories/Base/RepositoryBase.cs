using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportPlatform.Database
{
    public abstract class RepositoryBase<Entity> where Entity : EntityBase
    {
        protected abstract DbSet<Entity> _dbSet { get; }
        protected SupportPlatformDbContext _dbContext { get; }
        
        protected RepositoryBase(SupportPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool AddNew(Entity entity)
        {
            _dbSet.Add(entity);

            return SaveChanges();
        }

        public bool Delete(int id)
        {
            var foundEntity = _dbSet.FirstOrDefault(x => x.Id == id);

            if (foundEntity != null)
            {
                _dbSet.Remove(foundEntity);
                return SaveChanges();
            }

            return false;
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }
    }
}
