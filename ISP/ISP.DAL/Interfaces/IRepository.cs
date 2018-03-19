using System;
using System.Collections.Generic;
using ISP.DAL.Models;

namespace ISP.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : CommonEntity
    {
        ICollection<TEntity> GetAll(bool withDeleted = false);

        TEntity Get(int id,bool withDeleted = false);

        ICollection<TEntity> Find(Func<TEntity, bool> predicate, bool withDeleted = false);

        void Create(TEntity item);

        void Update(TEntity item);

        void Delete(int id);
    }
}
