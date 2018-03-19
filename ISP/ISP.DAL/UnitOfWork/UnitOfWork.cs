using ISP.DAL.Context;
using ISP.DAL.Identity;
using ISP.DAL.Interfaces;
using ISP.DAL.Models.Domain;
using ISP.DAL.Models.Identity;
using ISP.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ISP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ISPDBContext Database { get; }

        public virtual ApplicationUserManager UserManager { get; }

        public virtual ApplicationRoleManager RoleManager { get; }

        public virtual UserProfileRepository UserProfileRepository { get; }
        
        public virtual IRepository<Service> ServicesRepository { get; set; }

        public virtual IRepository<Plan> PlansRepository { get; set; }

        public virtual IRepository<Feature> FeaturesRepository { get; set; }

        public virtual IRepository<UserPlan> UserPlansRepository { get; set; }

        public virtual IRepository<UserService> UserServicesRepository { get; set; }
        
        public void SaveChanges()
        {
            Database.SaveChanges();
        }

        public UnitOfWork(ISPDBContext database)
        {
            Database = database;
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(Database));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(Database));
            UserProfileRepository = new UserProfileRepository(Database);
            ServicesRepository = new ServicesRepository(Database);
            PlansRepository = new PlansRepository(Database);
            FeaturesRepository = new FeaturesRepository(Database);
            UserPlansRepository = new UserPlansRepository(Database);
            UserServicesRepository = new UserServicesRepository(Database);
        }
    }
}
