namespace LMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialforce : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.News", "PublisherID", "dbo.AspNetUsers");
            DropIndex("dbo.News", new[] { "PublisherID" });
            AlterColumn("dbo.News", "EditedDate", c => c.DateTime());
            AlterColumn("dbo.News", "PublisherID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.News", "PublisherID");
            AddForeignKey("dbo.News", "PublisherID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "PublisherID", "dbo.AspNetUsers");
            DropIndex("dbo.News", new[] { "PublisherID" });
            AlterColumn("dbo.News", "PublisherID", c => c.String(maxLength: 128));
            AlterColumn("dbo.News", "EditedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.News", "PublisherID");
            AddForeignKey("dbo.News", "PublisherID", "dbo.AspNetUsers", "Id");
        }
    }
}
