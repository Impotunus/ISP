using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.DAL.Repositories
{
    public class PlansRepository : IRepository<Plan>
    {
        private ISPDBContext Database { get; }

        public PlansRepository(ISPDBContext database)
        {
            Database = database;
        }

        public virtual ICollection<Plan> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Plans.Where(t => t.IsDeleted == false)
                    .Include(t => t.Service)
                    .Include(t => t.Features)
                    .ToList();
            }
            return Database.Plans.ToList();
        }

        public virtual Plan Get(int id, bool withDeleted = false)
        {
            var item = Database.Plans
                .Include(t => t.Service)
                .Include(t => t.Features)
                .FirstOrDefault(t => t.Id == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public virtual ICollection<Plan> Find(Func<Plan, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Plans
                    .Include(t => t.Service)
                    .Include(t => t.Features)
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.Plans
                .Include(t => t.Service)
                .Include(t => t.Features)
                .Where(predicate)
                .ToList();
        }

        public virtual void Create(Plan item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(Plan item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var itemToDelete = Database.Plans.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }
    }
}
