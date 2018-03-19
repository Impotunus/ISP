using ISP.DAL.Identity;
using ISP.DAL.Models.Domain;
using ISP.DAL.Repositories;

namespace ISP.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ApplicationUserManager UserManager { get; }

        ApplicationRoleManager RoleManager { get; }

        UserProfileRepository UserProfileRepository { get; }
        
        IRepository<Service> ServicesRepository { get; set; }

        IRepository<Plan> PlansRepository { get; set; }

        IRepository<Feature> FeaturesRepository { get; set; }

        IRepository<UserPlan> UserPlansRepository { get; set; }

        IRepository<UserService> UserServicesRepository { get; set; }

        void SaveChanges();
    }
}
