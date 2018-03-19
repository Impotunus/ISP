using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.DAL.Repositories
{
    public class UserServicesRepository : IRepository<UserService>
    {
        private ISPDBContext Database { get; }

        public UserServicesRepository(ISPDBContext database)
        {
            Database = database;
        }

        public virtual ICollection<UserService> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserServices.Where(t => t.IsDeleted == false)
                    .Include(t => t.ApplicationUser)
                    .Include(t => t.Service)
                    .ToList();
            }
            return Database.UserServices.ToList();
        }

        public virtual UserService Get(int id, bool withDeleted = false)
        {
            var item = Database.UserServices
                .Include(t => t.ApplicationUser)
                .Include(t => t.Service)
                .FirstOrDefault(t => t.Id == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public virtual ICollection<UserService> Find(Func<UserService, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserServices
                    .Include(t => t.ApplicationUser)
                    .Include(t => t.Service)
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.UserServices
                .Include(t => t.ApplicationUser)
                .Include(t => t.Service)
                .Where(predicate)
                .ToList();
        }

        public virtual void Create(UserService item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(UserService item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var itemToDelete = Database.UserServices.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }
    }
}
