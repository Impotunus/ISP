using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.DAL.Repositories
{
    public class ServicesRepository : IRepository<Service>
    {
        private ISPDBContext Database { get; }

        public ServicesRepository(ISPDBContext database)
        {
            Database = database;
        }

        public virtual ICollection<Service> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Services.Where(t => t.IsDeleted == false)
                    .Include(t => t.Plans)
                    .Include(t => t.UserServices)
                    .ToList();
            }
            return Database.Services.ToList();
        }

        public virtual Service Get(int id, bool withDeleted = false)
        {
            var item = Database.Services
                .Include(t => t.Plans)
                .Include(t => t.UserServices)
                .FirstOrDefault(t => t.Id == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public virtual ICollection<Service> Find(Func<Service, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Services
                    .Include(t => t.Plans)
                    .Include(t => t.UserServices)
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.Services
                .Include(t => t.Plans)
                .Include(t => t.UserServices)
                .Where(predicate)
                .ToList();
        }

        public virtual void Create(Service item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(Service item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var itemToDelete = Database.Services.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }
    }
}
