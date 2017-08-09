namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentX : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Grades", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Grades", "Comment", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Grades", "Comment");
            DropColumn("dbo.Grades", "Date");
        }
    }
}
