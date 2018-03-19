using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Models.Identity;

namespace ISP.DAL.Repositories
{
    public class UserProfileRepository
    {
        private ISPDBContext Database { get; }

        public UserProfileRepository(ISPDBContext database)
        {
            Database = database;
        }

        public virtual ICollection<UserProfile> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserProfiles.Where(t => t.IsDeleted == false)
                    .Include(t => t.ApplicationUser)
                    .ToList();
            }
            return Database.UserProfiles.ToList();
        }

        public virtual UserProfile Get(string id, bool withDeleted = false)
        {
            var item = Database.UserProfiles
                .Include(t => t.ApplicationUser)
                .FirstOrDefault(t => t.ApplicationUserId == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public virtual ICollection<UserProfile> Find(Func<UserProfile, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.UserProfiles
                    .Include(t => t.ApplicationUser)
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.UserProfiles
                .Include(t => t.ApplicationUser)
                .Where(predicate)
                .ToList();
        }

        public virtual void Create(UserProfile item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public virtual void Update(UserProfile item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public virtual void Delete(int id)
        {
            var itemToDelete = Database.UserProfiles.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }

        public virtual void Dispose()
        {
            Database.Dispose();
        }
    }
}
