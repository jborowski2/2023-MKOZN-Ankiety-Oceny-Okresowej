namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "ApplicationUserID" });
            AddColumn("dbo.PoleAnkieties", "CommentID", c => c.Int());
            AddColumn("dbo.PoleAnkieties", "DzialComment_CommentID", c => c.Int());
            AddColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", c => c.Int());
            AlterColumn("dbo.Comments", "PoleAnkietyID", c => c.Int(nullable: false));
            CreateIndex("dbo.PoleAnkieties", "DzialComment_CommentID");
            CreateIndex("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            AddForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments", "CommentID");
            AddForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments", "CommentID");
            DropColumn("dbo.Comments", "ApplicationUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "ApplicationUserID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments");
            DropForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments");
            DropIndex("dbo.PoleAnkieties", new[] { "PrzelozonyComment_CommentID" });
            DropIndex("dbo.PoleAnkieties", new[] { "DzialComment_CommentID" });
            AlterColumn("dbo.Comments", "PoleAnkietyID", c => c.String());
            DropColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            DropColumn("dbo.PoleAnkieties", "DzialComment_CommentID");
            DropColumn("dbo.PoleAnkieties", "CommentID");
            CreateIndex("dbo.Comments", "ApplicationUserID");
            AddForeignKey("dbo.Comments", "ApplicationUserID", "dbo.AspNetUsers", "Id");
        }
    }
}
