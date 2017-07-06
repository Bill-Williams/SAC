namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduleToDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Schedules", "FromDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Schedules", "ToDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Schedules", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Schedules", "ToDate");
            DropColumn("dbo.Schedules", "FromDate");
        }
    }
}
