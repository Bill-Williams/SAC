namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Competitors", "CreatedDate", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Competitors", "CreatedDate");
        }
    }
}
