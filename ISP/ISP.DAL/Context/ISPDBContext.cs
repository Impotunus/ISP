using System.Data.Entity;
using ISP.DAL.Models.Domain;
using ISP.DAL.Models.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ISP.DAL.Context
{
    public class ISPDBContext : IdentityDbContext<ApplicationUser>
    {
        public ISPDBContext() : base("ISPDBContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Feature> Features { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<UserPlan> UserPlans { get; set; }

        public DbSet<UserService> UserServices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .HasKey(t => t.ApplicationUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(t => t.UserProfile)
                .WithRequired(t => t.ApplicationUser);
        }
    }
}
