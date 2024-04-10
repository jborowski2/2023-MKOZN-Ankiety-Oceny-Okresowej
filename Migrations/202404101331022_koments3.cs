namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class koments3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PoleAnkieties", "DzialComment_CommentID", c => c.Int());
            AddColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", c => c.Int());
            CreateIndex("dbo.PoleAnkieties", "DzialComment_CommentID");
            CreateIndex("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            AddForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments", "CommentID");
            AddForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments", "CommentID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments");
            DropForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments");
            DropIndex("dbo.PoleAnkieties", new[] { "PrzelozonyComment_CommentID" });
            DropIndex("dbo.PoleAnkieties", new[] { "DzialComment_CommentID" });
            DropColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            DropColumn("dbo.PoleAnkieties", "DzialComment_CommentID");
        }
    }
}
