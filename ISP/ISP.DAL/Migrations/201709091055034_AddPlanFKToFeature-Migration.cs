namespace ISP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlanFKToFeatureMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Features", "Plan_Id", "dbo.Plans");
            DropIndex("dbo.Features", new[] { "Plan_Id" });
            RenameColumn(table: "dbo.Features", name: "Plan_Id", newName: "PlanId");
            AlterColumn("dbo.Features", "PlanId", c => c.Int(nullable: false));
            CreateIndex("dbo.Features", "PlanId");
            AddForeignKey("dbo.Features", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Features", "PlanId", "dbo.Plans");
            DropIndex("dbo.Features", new[] { "PlanId" });
            AlterColumn("dbo.Features", "PlanId", c => c.Int());
            RenameColumn(table: "dbo.Features", name: "PlanId", newName: "Plan_Id");
            CreateIndex("dbo.Features", "Plan_Id");
            AddForeignKey("dbo.Features", "Plan_Id", "dbo.Plans", "Id");
        }
    }
}
