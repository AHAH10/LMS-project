namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Courses : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Courses", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "Name", c => c.String(nullable: false));
        }
    }
}
