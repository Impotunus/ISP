using System.Data.Entity.Migrations;

namespace ISP.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ISP.DAL.Context.ISPDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ISP.DAL.Context.ISPDBContext context)
        {

        }
    }
}
