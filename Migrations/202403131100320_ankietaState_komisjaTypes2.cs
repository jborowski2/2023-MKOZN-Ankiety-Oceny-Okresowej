namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankietaState_komisjaTypes2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Komisjas", "KomisjaName", c => c.String());
            DropColumn("dbo.Ankietas", "a");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ankietas", "a", c => c.Int(nullable: false));
            DropColumn("dbo.Komisjas", "KomisjaName");
        }
    }
}
