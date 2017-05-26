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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 2),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 250),
                        Known = c.Boolean(nullable: false),
                        MaximumYardage = c.Int(nullable: false),
                        Restrictions = c.String(nullable: false, maxLength: 100),
                        ColorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId, cascadeDelete: true)
                .Index(t => t.ColorId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HexCode = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 100),
                        CityStateZip = c.String(maxLength: 100),
                        Contact = c.String(maxLength: 250),
                        Phone = c.String(maxLength: 100),
                        Email = c.String(maxLength: 100),
                        Website = c.String(maxLength: 100),
                        Directions = c.String(maxLength: 2000),
                        IconFileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Competitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArcherId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Bonus = c.Int(nullable: false),
                        Tournament_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Archers", t => t.ArcherId, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id)
                .Index(t => t.ArcherId)
                .Index(t => t.ClassId)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClubId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.ClubId, cascadeDelete: true)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schedules", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.ScheduleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Tournaments", "ScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Competitors", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Schedules", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Competitors", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Competitors", "ArcherId", "dbo.Archers");
            DropForeignKey("dbo.Classes", "ColorId", "dbo.Colors");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Tournaments", new[] { "ScheduleId" });
            DropIndex("dbo.Schedules", new[] { "ClubId" });
            DropIndex("dbo.Competitors", new[] { "Tournament_Id" });
            DropIndex("dbo.Competitors", new[] { "ClassId" });
            DropIndex("dbo.Competitors", new[] { "ArcherId" });
            DropIndex("dbo.Classes", new[] { "ColorId" });
            DropTable("dbo.Users");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Schedules");
            DropTable("dbo.Roles");
            DropTable("dbo.Competitors");
            DropTable("dbo.Clubs");
            DropTable("dbo.Colors");
            DropTable("dbo.Classes");
            DropTable("dbo.Archers");
        }
    }
}
