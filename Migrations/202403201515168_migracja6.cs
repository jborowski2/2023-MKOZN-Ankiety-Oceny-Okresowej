namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Pracowniks", "GrupaId");
            DropColumn("dbo.Pracowniks", "StanowiskoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pracowniks", "StanowiskoId", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "GrupaId", c => c.Int(nullable: false));
        }
    }
}
