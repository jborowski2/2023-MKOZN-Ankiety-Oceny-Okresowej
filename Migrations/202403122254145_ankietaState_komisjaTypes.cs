namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ankietaState_komisjaTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ankietas", "AnkietaState", c => c.Int(nullable: false));
            AddColumn("dbo.Komisjas", "KomisjaType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Komisjas", "KomisjaType");
            DropColumn("dbo.Ankietas", "AnkietaState");
        }
    }
}
