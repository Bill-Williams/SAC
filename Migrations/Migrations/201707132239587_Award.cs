namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Award : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Awards",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Icon = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Competitors", "AwardId", c => c.Guid());
            CreateIndex("dbo.Competitors", "AwardId");
            AddForeignKey("dbo.Competitors", "AwardId", "dbo.Awards", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Competitors", "AwardId", "dbo.Awards");
            DropIndex("dbo.Competitors", new[] { "AwardId" });
            DropColumn("dbo.Competitors", "AwardId");
            DropTable("dbo.Awards");
        }
    }
}
