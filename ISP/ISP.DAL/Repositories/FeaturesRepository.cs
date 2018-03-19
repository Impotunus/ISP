using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ISP.DAL.Context;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;

namespace ISP.DAL.Repositories
{
    public class FeaturesRepository : IRepository<Feature>
    {
        private ISPDBContext Database { get; }

        public FeaturesRepository(ISPDBContext database)
        {
            Database = database;
        }

        public ICollection<Feature> GetAll(bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Features.Where(t => t.IsDeleted == false)
                    .ToList();
            }
            return Database.Features.ToList();
        }

        public Feature Get(int id, bool withDeleted = false)
        {
            var item = Database.Features
                .FirstOrDefault(t => t.Id == id);

            if (withDeleted) return item;
            if (item != null)
            {
                return item.IsDeleted ? null : item;
            }
            return null;
        }

        public ICollection<Feature> Find(Func<Feature, bool> predicate, bool withDeleted = false)
        {
            if (!withDeleted)
            {
                return Database.Features
                    .Where(t => t.IsDeleted == false)
                    .Where(predicate)
                    .ToList();
            }

            return Database.Features
                .Where(predicate)
                .ToList();
        }

        public void Create(Feature item)
        {
            Database.Entry(item).State = EntityState.Added;
        }

        public void Update(Feature item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var itemToDelete = Database.Features.Find(id);
            if (itemToDelete != null) itemToDelete.IsDeleted = true;
        }
    }
}
