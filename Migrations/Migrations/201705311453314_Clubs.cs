namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Clubs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clubs", "Phone", c => c.String());
            AlterColumn("dbo.Clubs", "Email", c => c.String());
            AlterColumn("dbo.Clubs", "Website", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clubs", "Website", c => c.String(maxLength: 100));
            AlterColumn("dbo.Clubs", "Email", c => c.String(maxLength: 100));
            AlterColumn("dbo.Clubs", "Phone", c => c.String(maxLength: 100));
        }
    }
}
