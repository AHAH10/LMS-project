namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class document : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "CourseID", "dbo.Courses");
            DropIndex("dbo.Documents", new[] { "CourseID" });
            AlterColumn("dbo.Documents", "CourseID", c => c.Int(nullable: false));
            CreateIndex("dbo.Documents", "CourseID");
            AddForeignKey("dbo.Documents", "CourseID", "dbo.Courses", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "CourseID", "dbo.Courses");
            DropIndex("dbo.Documents", new[] { "CourseID" });
            AlterColumn("dbo.Documents", "CourseID", c => c.Int());
            CreateIndex("dbo.Documents", "CourseID");
            AddForeignKey("dbo.Documents", "CourseID", "dbo.Courses", "ID");
        }
    }
}
