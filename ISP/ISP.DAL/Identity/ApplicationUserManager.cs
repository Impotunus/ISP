using ISP.DAL.Models.Identity;
using Microsoft.AspNet.Identity;

namespace ISP.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}
