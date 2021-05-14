namespace APIReview.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Begin : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        EmployeeID = c.String(nullable: false, maxLength: 128),
                        EmployeeName = c.String(),
                        EmploymentStatusID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.EmploymentStatus", t => t.EmploymentStatusID, cascadeDelete: true)
                .Index(t => t.EmploymentStatusID);
            
            CreateTable(
                "dbo.EmploymentStatus",
                c => new
                    {
                        EmploymentStatusID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EmploymentStatusID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employee", "EmploymentStatusID", "dbo.EmploymentStatus");
            DropIndex("dbo.Employee", new[] { "EmploymentStatusID" });
            DropTable("dbo.EmploymentStatus");
            DropTable("dbo.Employee");
        }
    }
}
