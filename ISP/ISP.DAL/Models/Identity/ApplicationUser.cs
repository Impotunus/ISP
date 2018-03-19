using System.Collections.Generic;
using ISP.DAL.Models.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ISP.DAL.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<UserService> UserServices { get; set; }

        public virtual ICollection<UserPlan> UserPlans { get; set; }

        public bool AdminBanned { get; set; }
    }
}
