namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 2),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 250),
                        Known = c.Boolean(nullable: false),
                        MaximumYardage = c.Int(nullable: false),
                        Restrictions = c.String(nullable: false, maxLength: 100),
                        ColorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .Index(t => t.ColorId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HexCode = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(maxLength: 15),
                        Address = c.String(maxLength: 100),
                        CityStateZip = c.String(maxLength: 100),
                        Contact = c.String(maxLength: 250),
                        Phone = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        Directions = c.String(maxLength: 2000),
                        IconFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Competitors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ArcherId = c.Guid(nullable: false),
                        ClassId = c.Guid(nullable: false),
                        Score = c.Int(nullable: false),
                        Bonus = c.Int(nullable: false),
                        Tournament_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archers", t => t.ArcherId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .Index(t => t.ArcherId)
                .Index(t => t.ClassId)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
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
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ClubId = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.ClubId, cascadeDelete: true)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ScheduleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.ScheduleId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Competitors", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Schedules", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Competitors", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Competitors", "ArcherId", "dbo.Archers");
            DropForeignKey("dbo.Classes", "ColorId", "dbo.Colors");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Tournaments", new[] { "ScheduleId" });
            DropIndex("dbo.Schedules", new[] { "ClubId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Competitors", new[] { "Tournament_Id" });
            DropIndex("dbo.Competitors", new[] { "ClassId" });
            DropIndex("dbo.Competitors", new[] { "ArcherId" });
            DropIndex("dbo.Classes", new[] { "ColorId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Schedules");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Competitors");
            DropTable("dbo.Clubs");
            DropTable("dbo.Colors");
            DropTable("dbo.Classes");
            DropTable("dbo.Archers");
        }
    }
}
