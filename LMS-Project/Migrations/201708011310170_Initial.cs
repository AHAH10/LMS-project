namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classrooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Location = c.String(nullable: false, maxLength: 255),
                        Remarks = c.String(maxLength: 255),
                        AmountStudentsMax = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WeekDay = c.Int(nullable: false),
                        BeginningTime = c.String(),
                        EndingTime = c.String(nullable: false),
                        CourseID = c.Int(nullable: false),
                        ClassroomID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Classrooms", t => t.ClassroomID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .Index(t => t.CourseID)
                .Index(t => t.ClassroomID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SubjectID = c.Int(nullable: false),
                        TeacherID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TeacherID)
                .Index(t => t.SubjectID)
                .Index(t => t.TeacherID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DocumentName = c.String(nullable: false),
                        DocumentContent = c.Binary(nullable: false),
                        UploadingDate = c.DateTime(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                        CourseID = c.Int(nullable: false),
                        RoleID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CourseID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.AspNetUsers",
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
                        FirstName = c.String(),
                        LastName = c.String(),
                        BirthDate = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CourseGrade = c.Int(nullable: false),
                        DocumentID = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Documents", t => t.DocumentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.DocumentID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PublishingDate = c.DateTime(nullable: false),
                        LastEditedDate = c.DateTime(nullable: false),
                        Title = c.String(),
                        NewsContent = c.String(),
                        PublisherID = c.String(maxLength: 128),
                        EditedByID = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.EditedByID)
                .ForeignKey("dbo.AspNetUsers", t => t.PublisherID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.PublisherID)
                .Index(t => t.EditedByID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserSchedules",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Schedule_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Schedule_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Schedules", t => t.Schedule_ID, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Schedule_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Schedules", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "TeacherID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Documents", "RoleID", "dbo.AspNetRoles");
            DropForeignKey("dbo.Documents", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSchedules", "Schedule_ID", "dbo.Schedules");
            DropForeignKey("dbo.UserSchedules", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "PublisherID", "dbo.AspNetUsers");
            DropForeignKey("dbo.News", "EditedByID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Grades", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Grades", "DocumentID", "dbo.Documents");
            DropForeignKey("dbo.Documents", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.Schedules", "ClassroomID", "dbo.Classrooms");
            DropIndex("dbo.UserSchedules", new[] { "Schedule_ID" });
            DropIndex("dbo.UserSchedules", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.News", new[] { "User_Id" });
            DropIndex("dbo.News", new[] { "EditedByID" });
            DropIndex("dbo.News", new[] { "PublisherID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Grades", new[] { "User_Id" });
            DropIndex("dbo.Grades", new[] { "DocumentID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Documents", new[] { "RoleID" });
            DropIndex("dbo.Documents", new[] { "CourseID" });
            DropIndex("dbo.Documents", new[] { "UserID" });
            DropIndex("dbo.Courses", new[] { "TeacherID" });
            DropIndex("dbo.Courses", new[] { "SubjectID" });
            DropIndex("dbo.Schedules", new[] { "ClassroomID" });
            DropIndex("dbo.Schedules", new[] { "CourseID" });
            DropTable("dbo.UserSchedules");
            DropTable("dbo.Subjects");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.News");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Grades");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Documents");
            DropTable("dbo.Courses");
            DropTable("dbo.Schedules");
            DropTable("dbo.Classrooms");
        }
    }
}
