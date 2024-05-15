namespace OOP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adres",
                c => new
                    {
                        AdresID = c.Int(nullable: false, identity: true),
                        Ulica = c.String(),
                        Numer = c.String(),
                        KodPocztowy = c.String(),
                        Miasto = c.String(),
                        ApplicationUserID = c.String(),
                    })
                .PrimaryKey(t => t.AdresID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Adres_AdresID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adres", t => t.Adres_AdresID, cascadeDelete: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Adres_AdresID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Ankietas",
                c => new
                    {
                        AnkietaID = c.Int(nullable: false, identity: true),
                        AnkietaState = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        PracownikID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnkietaID)
                .ForeignKey("dbo.Pracowniks", t => t.PracownikID, cascadeDelete: true)
                .Index(t => t.PracownikID);
            
            CreateTable(
                "dbo.Pracowniks",
                c => new
                    {
                        PracownikID = c.Int(nullable: false, identity: true),
                        Imie = c.String(),
                        Nazwisko = c.String(),
                        Stopien = c.String(),
                        Tytul = c.String(),
                        NumerTelefonu = c.Int(nullable: false),
                        KomisjaID = c.Int(),
                        PrzelozonyID = c.Int(nullable: false),
                        ApplicationUserID = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Przelozony_PracownikID = c.Int(),
                    })
                .PrimaryKey(t => t.PracownikID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Komisjas", t => t.KomisjaID)
                .ForeignKey("dbo.Pracowniks", t => t.Przelozony_PracownikID)
                .Index(t => t.KomisjaID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Przelozony_PracownikID);
            
            CreateTable(
                "dbo.Komisjas",
                c => new
                    {
                        KomisjaID = c.Int(nullable: false, identity: true),
                        KomisjaName = c.String(),
                        KomisjaType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KomisjaID);
            
            CreateTable(
                "dbo.Osiagniecies",
                c => new
                    {
                        OsiagniecieID = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Szczegoly = c.String(),
                        DzialID = c.Int(nullable: false),
                        PracownikID = c.Int(nullable: false),
                        Nazwa = c.String(),
                    })
                .PrimaryKey(t => t.OsiagniecieID)
                .ForeignKey("dbo.Dzials", t => t.DzialID, cascadeDelete: true)
                .ForeignKey("dbo.Pracowniks", t => t.PracownikID, cascadeDelete: true)
                .Index(t => t.DzialID)
                .Index(t => t.PracownikID);
            
            CreateTable(
                "dbo.Dzials",
                c => new
                    {
                        DzialID = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DzialID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Ocenas",
                c => new
                    {
                        OcenaID = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Stopien = c.String(),
                        DzialID = c.Int(nullable: false),
                        KomisjaID = c.Int(nullable: false),
                        PracownikID = c.Int(nullable: false),
                        HistoriaOcenID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OcenaID)
                .ForeignKey("dbo.Dzials", t => t.DzialID, cascadeDelete: true)
                .ForeignKey("dbo.Komisjas", t => t.KomisjaID, cascadeDelete: true)
                .ForeignKey("dbo.Pracowniks", t => t.PracownikID, cascadeDelete: true)
                .Index(t => t.DzialID)
                .Index(t => t.KomisjaID)
                .Index(t => t.PracownikID);
            
            CreateTable(
                "dbo.StronaAnkieties",
                c => new
                    {
                        StronaAnkietyID = c.Int(nullable: false, identity: true),
                        DzialID = c.Int(nullable: false),
                        AnkietaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StronaAnkietyID)
                .ForeignKey("dbo.Dzials", t => t.DzialID, cascadeDelete: true)
                .ForeignKey("dbo.Ankietas", t => t.AnkietaID, cascadeDelete: true)
                .Index(t => t.DzialID)
                .Index(t => t.AnkietaID);
            
            CreateTable(
                "dbo.PoleAnkieties",
                c => new
                    {
                        PoleAnkietyID = c.Int(nullable: false, identity: true),
                        StronaAnkietyID = c.Int(nullable: false),
                        LiczbaPunktow = c.Int(nullable: false),
                        Tresc = c.String(),
                        Organizacyjne = c.Boolean(nullable: false),
                        MaksymalnaIloscPunktow = c.String(),
                        AttachmentID = c.Int(),
                        CommentID = c.Int(),
                        DzialComment_CommentID = c.Int(),
                        PrzelozonyComment_CommentID = c.Int(),
                    })
                .PrimaryKey(t => t.PoleAnkietyID)
                .ForeignKey("dbo.Comments", t => t.DzialComment_CommentID)
                .ForeignKey("dbo.Comments", t => t.PrzelozonyComment_CommentID)
                .ForeignKey("dbo.StronaAnkieties", t => t.StronaAnkietyID, cascadeDelete: true)
                .Index(t => t.StronaAnkietyID)
                .Index(t => t.DzialComment_CommentID)
                .Index(t => t.PrzelozonyComment_CommentID);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileType = c.String(),
                        FilePath = c.String(),
                        PoleAnkietyID = c.Int(nullable: false),
                        PoleAnkiety_PoleAnkietyID = c.Int(),
                        PoleAnkiety_PoleAnkietyID1 = c.Int(),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.PoleAnkieties", t => t.PoleAnkiety_PoleAnkietyID)
                .ForeignKey("dbo.PoleAnkieties", t => t.PoleAnkiety_PoleAnkietyID1)
                .Index(t => t.PoleAnkiety_PoleAnkietyID)
                .Index(t => t.PoleAnkiety_PoleAnkietyID1);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false),
                        CommentText = c.String(),
                        PoleAnkietyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.PoleAnkieties", t => t.CommentID)
                .Index(t => t.CommentID);
            
            CreateTable(
                "dbo.PracaDyplomowas",
                c => new
                    {
                        PracaDyplomowaID = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Temat = c.String(),
                        PracownikID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PracaDyplomowaID)
                .ForeignKey("dbo.Pracowniks", t => t.PracownikID, cascadeDelete: true)
                .Index(t => t.PracownikID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Schemats",
                c => new
                    {
                        SchematID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MaxPoints = c.String(),
                        IsOrganizational = c.Boolean(nullable: false),
                        DzialID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SchematID)
                .ForeignKey("dbo.Dzials", t => t.DzialID, cascadeDelete: true)
                .Index(t => t.DzialID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schemats", "DzialID", "dbo.Dzials");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.StronaAnkieties", "AnkietaID", "dbo.Ankietas");
            DropForeignKey("dbo.Pracowniks", "Przelozony_PracownikID", "dbo.Pracowniks");
            DropForeignKey("dbo.PracaDyplomowas", "PracownikID", "dbo.Pracowniks");
            DropForeignKey("dbo.Osiagniecies", "PracownikID", "dbo.Pracowniks");
            DropForeignKey("dbo.PoleAnkieties", "StronaAnkietyID", "dbo.StronaAnkieties");
            DropForeignKey("dbo.PoleAnkieties", "PrzelozonyComment_CommentID", "dbo.Comments");
            DropForeignKey("dbo.PoleAnkieties", "DzialComment_CommentID", "dbo.Comments");
            DropForeignKey("dbo.Comments", "CommentID", "dbo.PoleAnkieties");
            DropForeignKey("dbo.Attachments", "PoleAnkiety_PoleAnkietyID1", "dbo.PoleAnkieties");
            DropForeignKey("dbo.Attachments", "PoleAnkiety_PoleAnkietyID", "dbo.PoleAnkieties");
            DropForeignKey("dbo.StronaAnkieties", "DzialID", "dbo.Dzials");
            DropForeignKey("dbo.Osiagniecies", "DzialID", "dbo.Dzials");
            DropForeignKey("dbo.Ocenas", "PracownikID", "dbo.Pracowniks");
            DropForeignKey("dbo.Ocenas", "KomisjaID", "dbo.Komisjas");
            DropForeignKey("dbo.Ocenas", "DzialID", "dbo.Dzials");
            DropForeignKey("dbo.Dzials", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pracowniks", "KomisjaID", "dbo.Komisjas");
            DropForeignKey("dbo.Pracowniks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ankietas", "PracownikID", "dbo.Pracowniks");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Adres_AdresID", "dbo.Adres");
            DropIndex("dbo.Schemats", new[] { "DzialID" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.PracaDyplomowas", new[] { "PracownikID" });
            DropIndex("dbo.Comments", new[] { "CommentID" });
            DropIndex("dbo.Attachments", new[] { "PoleAnkiety_PoleAnkietyID1" });
            DropIndex("dbo.Attachments", new[] { "PoleAnkiety_PoleAnkietyID" });
            DropIndex("dbo.PoleAnkieties", new[] { "PrzelozonyComment_CommentID" });
            DropIndex("dbo.PoleAnkieties", new[] { "DzialComment_CommentID" });
            DropIndex("dbo.PoleAnkieties", new[] { "StronaAnkietyID" });
            DropIndex("dbo.StronaAnkieties", new[] { "AnkietaID" });
            DropIndex("dbo.StronaAnkieties", new[] { "DzialID" });
            DropIndex("dbo.Ocenas", new[] { "PracownikID" });
            DropIndex("dbo.Ocenas", new[] { "KomisjaID" });
            DropIndex("dbo.Ocenas", new[] { "DzialID" });
            DropIndex("dbo.Dzials", new[] { "ApplicationUserID" });
            DropIndex("dbo.Osiagniecies", new[] { "PracownikID" });
            DropIndex("dbo.Osiagniecies", new[] { "DzialID" });
            DropIndex("dbo.Pracowniks", new[] { "Przelozony_PracownikID" });
            DropIndex("dbo.Pracowniks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Pracowniks", new[] { "KomisjaID" });
            DropIndex("dbo.Ankietas", new[] { "PracownikID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Adres_AdresID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropTable("dbo.Schemats");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.PracaDyplomowas");
            DropTable("dbo.Comments");
            DropTable("dbo.Attachments");
            DropTable("dbo.PoleAnkieties");
            DropTable("dbo.StronaAnkieties");
            DropTable("dbo.Ocenas");
            DropTable("dbo.Dzials");
            DropTable("dbo.Osiagniecies");
            DropTable("dbo.Komisjas");
            DropTable("dbo.Pracowniks");
            DropTable("dbo.Ankietas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Adres");
        }
    }
}
