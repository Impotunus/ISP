namespace ISP.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePlanFKFromFeatureMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Features", "PlanId", "dbo.Plans");
            DropIndex("dbo.Features", new[] { "PlanId" });
            RenameColumn(table: "dbo.Features", name: "PlanId", newName: "Plan_Id");
            AlterColumn("dbo.Features", "Plan_Id", c => c.Int());
            CreateIndex("dbo.Features", "Plan_Id");
            AddForeignKey("dbo.Features", "Plan_Id", "dbo.Plans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Features", "Plan_Id", "dbo.Plans");
            DropIndex("dbo.Features", new[] { "Plan_Id" });
            AlterColumn("dbo.Features", "Plan_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Features", name: "Plan_Id", newName: "PlanId");
            CreateIndex("dbo.Features", "PlanId");
            AddForeignKey("dbo.Features", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
        }
    }
}
