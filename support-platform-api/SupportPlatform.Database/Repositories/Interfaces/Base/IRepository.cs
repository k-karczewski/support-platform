using System;
using System.Collections.Generic;
using System.Text;

namespace SupportPlatform.Database
{
    public interface IRepository<Entity> where Entity : EntityBase
    {
        bool AddNew(Entity entity);
        bool Delete(int id);
    }
}
