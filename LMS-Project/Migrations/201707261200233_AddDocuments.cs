namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        DocumentName = c.String(),
                        DocumentContent = c.String(),
                        UploadingDate = c.DateTime(nullable: false),
                        CourseID = c.Int(nullable: false),
                        RoleID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleID)
                .Index(t => t.UserID)
                .Index(t => t.CourseID)
                .Index(t => t.RoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles");
            DropForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "CourseID", "dbo.Courses");
            DropIndex("dbo.Documents", new[] { "RoleID" });
            DropIndex("dbo.Documents", new[] { "CourseID" });
            DropIndex("dbo.Documents", new[] { "UserID" });
            DropTable("dbo.Documents");
        }
    }
}
