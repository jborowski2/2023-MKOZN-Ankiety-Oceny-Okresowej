namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pracowniks", "GrupaId", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "StanowiskoId", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "Grupa", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "Stanowisko", c => c.Int(nullable: false));
            DropColumn("dbo.Pracowniks", "pracownikGrupa");
            DropColumn("dbo.Pracowniks", "pracownikStanowisko");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pracowniks", "pracownikStanowisko", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "pracownikGrupa", c => c.Int(nullable: false));
            DropColumn("dbo.Pracowniks", "Stanowisko");
            DropColumn("dbo.Pracowniks", "Grupa");
            DropColumn("dbo.Pracowniks", "StanowiskoId");
            DropColumn("dbo.Pracowniks", "GrupaId");
        }
    }
}
