using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.DAL.Repositories
{
    public class UserPlansRepository : IRepository<UserPlan>
    {
        private ISPDBContext Database { get; }

        public UserPlansRepository(ISPDBContext database)
        {
            Database = database;
        }

        public virtual ICollection<UserPlan> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserPlans.Where(t => t.IsDeleted == false)
                    .Include(t => t.ApplicationUser)
                    .Include(t => t.Plan)
                    .ToList();
            }
            return Database.UserPlans.ToList();
        }

        public virtual UserPlan Get(int id, bool withDeleted = false)
        {
            var item = Database.UserPlans
                .Include(t => t.ApplicationUser)
                .Include(t => t.Plan)
                .FirstOrDefault(t => t.Id == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public virtual ICollection<UserPlan> Find(Func<UserPlan, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserPlans
                    .Include(t => t.ApplicationUser)
                    .Include(t => t.Plan)
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.UserPlans
                .Include(t => t.ApplicationUser)
                .Include(t => t.Plan)
                .Where(predicate)
                .ToList();
        }

        public virtual void Create(UserPlan item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(UserPlan item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var itemToDelete = Database.UserPlans.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }
    }
}
