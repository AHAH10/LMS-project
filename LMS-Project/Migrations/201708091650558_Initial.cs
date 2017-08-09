namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "Name");
        }
    }
}
