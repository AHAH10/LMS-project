namespace LMS_Project.Migrations.LMSMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "library.Classrooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Location = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "library.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        TeacherID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("library.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("library.AspNetUsers", t => t.TeacherID)
                .Index(t => t.SubjectId)
                .Index(t => t.TeacherID);
            
            CreateTable(
                "library.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "library.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "library.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("library.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "library.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("library.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "library.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("library.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("library.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "library.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("library.AspNetUserRoles", "RoleId", "library.AspNetRoles");
            DropForeignKey("library.Courses", "TeacherID", "library.AspNetUsers");
            DropForeignKey("library.AspNetUserRoles", "UserId", "library.AspNetUsers");
            DropForeignKey("library.AspNetUserLogins", "UserId", "library.AspNetUsers");
            DropForeignKey("library.AspNetUserClaims", "UserId", "library.AspNetUsers");
            DropForeignKey("library.Courses", "SubjectId", "library.Subjects");
            DropIndex("library.AspNetRoles", "RoleNameIndex");
            DropIndex("library.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("library.AspNetUserRoles", new[] { "UserId" });
            DropIndex("library.AspNetUserLogins", new[] { "UserId" });
            DropIndex("library.AspNetUserClaims", new[] { "UserId" });
            DropIndex("library.AspNetUsers", "UserNameIndex");
            DropIndex("library.Courses", new[] { "TeacherID" });
            DropIndex("library.Courses", new[] { "SubjectId" });
            DropTable("library.AspNetRoles");
            DropTable("library.AspNetUserRoles");
            DropTable("library.AspNetUserLogins");
            DropTable("library.AspNetUserClaims");
            DropTable("library.AspNetUsers");
            DropTable("library.Subjects");
            DropTable("library.Courses");
            DropTable("library.Classrooms");
        }
    }
}
