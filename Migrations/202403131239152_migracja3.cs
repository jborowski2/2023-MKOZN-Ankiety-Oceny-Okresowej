namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migracja3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pracowniks", name: "Komisja_KomisjaID", newName: "KomisjaID");
            RenameIndex(table: "dbo.Pracowniks", name: "IX_Komisja_KomisjaID", newName: "IX_KomisjaID");
            AddColumn("dbo.Ankietas", "AnkietaState", c => c.Int(nullable: false));
            AddColumn("dbo.Komisjas", "KomisjaName", c => c.String());
            AddColumn("dbo.Komisjas", "KomisjaType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Komisjas", "KomisjaType");
            DropColumn("dbo.Komisjas", "KomisjaName");
            DropColumn("dbo.Ankietas", "AnkietaState");
            RenameIndex(table: "dbo.Pracowniks", name: "IX_KomisjaID", newName: "IX_Komisja_KomisjaID");
            RenameColumn(table: "dbo.Pracowniks", name: "KomisjaID", newName: "Komisja_KomisjaID");
        }
    }
}
