namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles");
            DropIndex("dbo.Documents", new[] { "UserID" });
            DropIndex("dbo.Documents", new[] { "RoleID" });
            AlterColumn("dbo.Schedules", "BeginningTime", c => c.String());
            AlterColumn("dbo.Schedules", "EndingTime", c => c.String(nullable: false));
            AlterColumn("dbo.Documents", "UserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Documents", "DocumentName", c => c.String(nullable: false));
            AlterColumn("dbo.Documents", "DocumentContent", c => c.Binary(nullable: false));
            AlterColumn("dbo.Documents", "RoleID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Documents", "UserID");
            CreateIndex("dbo.Documents", "RoleID");
            AddForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles");
            DropForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "RoleID" });
            DropIndex("dbo.Documents", new[] { "UserID" });
            AlterColumn("dbo.Documents", "RoleID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Documents", "DocumentContent", c => c.Binary());
            AlterColumn("dbo.Documents", "DocumentName", c => c.String());
            AlterColumn("dbo.Documents", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Schedules", "EndingTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Schedules", "BeginningTime", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Documents", "RoleID");
            CreateIndex("dbo.Documents", "UserID");
            AddForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles", "Id");
            AddForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
