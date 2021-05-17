namespace DewaldReview.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExternalRef : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employee", "ExternalReference", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employee", "ExternalReference");
        }
    }
}
