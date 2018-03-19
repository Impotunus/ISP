using ISP.DAL.Models.Identity;
using Microsoft.AspNet.Identity;

namespace ISP.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }

    }
}
