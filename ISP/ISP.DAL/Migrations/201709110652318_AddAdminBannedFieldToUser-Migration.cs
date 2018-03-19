namespace ISP.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAdminBannedFieldToUserMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AdminBanned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AdminBanned");
        }
    }
}
