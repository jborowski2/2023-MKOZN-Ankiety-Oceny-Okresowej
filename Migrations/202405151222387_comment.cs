namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "CommentID", "dbo.PoleAnkieties");
            DropForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments");
            DropForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments");
            DropIndex("dbo.PoleAnkieties", new[] { "DzialComment_CommentID" });
            DropIndex("dbo.PoleAnkieties", new[] { "PrzelozonyComment_CommentID" });
            DropIndex("dbo.Comments", new[] { "CommentID" });
            AddColumn("dbo.PoleAnkieties", "PracownikComment", c => c.String());
            DropColumn("dbo.PoleAnkieties", "CommentID");
            DropColumn("dbo.PoleAnkieties", "DzialComment_CommentID");
            DropColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            DropTable("dbo.Comments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false),
                        CommentText = c.String(),
                        PoleAnkietyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID);
            
            AddColumn("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", c => c.Int());
            AddColumn("dbo.PoleAnkieties", "DzialComment_CommentID", c => c.Int());
            AddColumn("dbo.PoleAnkieties", "CommentID", c => c.Int());
            DropColumn("dbo.PoleAnkieties", "PracownikComment");
            CreateIndex("dbo.Comments", "CommentID");
            CreateIndex("dbo.PoleAnkieties", "PrzelozonyComment_CommentID");
            CreateIndex("dbo.PoleAnkieties", "DzialComment_CommentID");
            AddForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments", "CommentID");
            AddForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments", "CommentID");
            AddForeignKey("dbo.Comments", "CommentID", "dbo.PoleAnkieties", "PoleAnkietyID");
        }
    }
}
