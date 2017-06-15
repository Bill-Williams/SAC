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
                        GroupId = c.Guid(nullable: false),
                        Description = c.String(nullable: false, maxLength: 250),
                        Known = c.Boolean(nullable: false),
                        MaximumYardage = c.Int(nullable: false),
                        Restrictions = c.String(maxLength: 100),
                        ColorId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId)
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
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ShortName = c.String(nullable: false, maxLength: 15),
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
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
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
                "dbo.Competitors",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        Bonus = c.Int(nullable: false),
                        Archer_Id = c.Guid(),
                        Class_Id = c.Guid(),
                        Tournament_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archers", t => t.Archer_Id)
                .ForeignKey("dbo.Classes", t => t.Class_Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .Index(t => t.Archer_Id)
                .Index(t => t.Class_Id)
                .Index(t => t.Tournament_Id);
            
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
                        Completed = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.AspNetUserClubs",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ClubId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Clubs", t => t.ClubId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ClubId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tournaments", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Competitors", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Schedules", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Competitors", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.Competitors", "Archer_Id", "dbo.Archers");
            DropForeignKey("dbo.AspNetUserClubs", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.AspNetUserClubs", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classes", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Classes", "ColorId", "dbo.Colors");
            DropIndex("dbo.AspNetUserClubs", new[] { "ClubId" });
            DropIndex("dbo.AspNetUserClubs", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Tournaments", new[] { "ScheduleId" });
            DropIndex("dbo.Schedules", new[] { "ClubId" });
            DropIndex("dbo.Competitors", new[] { "Tournament_Id" });
            DropIndex("dbo.Competitors", new[] { "Class_Id" });
            DropIndex("dbo.Competitors", new[] { "Archer_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Classes", new[] { "ColorId" });
            DropIndex("dbo.Classes", new[] { "GroupId" });
            DropTable("dbo.AspNetUserClubs");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Schedules");
            DropTable("dbo.Competitors");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Clubs");
            DropTable("dbo.Groups");
            DropTable("dbo.Colors");
            DropTable("dbo.Classes");
            DropTable("dbo.Archers");
        }
    }
}
