namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pracowniks", "pracownikGrupa", c => c.Int(nullable: false));
            AddColumn("dbo.Pracowniks", "pracownikStanowisko", c => c.Int(nullable: false));
            DropColumn("dbo.Pracowniks", "Stopien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pracowniks", "Stopien", c => c.String());
            DropColumn("dbo.Pracowniks", "pracownikStanowisko");
            DropColumn("dbo.Pracowniks", "pracownikGrupa");
        }
    }
}
