namespace ISP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPlanStatusAndWillUnsubMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPlans", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.UserPlans", "WillUnsubscribe", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserPlans", "WillUnsubscribe");
            DropColumn("dbo.UserPlans", "Status");
        }
    }
}
