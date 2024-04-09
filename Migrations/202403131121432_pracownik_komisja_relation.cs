namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pracownik_komisja_relation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pracowniks", name: "Komisja_KomisjaID", newName: "KomisjaID");
            RenameIndex(table: "dbo.Pracowniks", name: "IX_Komisja_KomisjaID", newName: "IX_KomisjaID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Pracowniks", name: "IX_KomisjaID", newName: "IX_Komisja_KomisjaID");
            RenameColumn(table: "dbo.Pracowniks", name: "KomisjaID", newName: "Komisja_KomisjaID");
        }
    }
}
