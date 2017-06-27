namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactClubLink : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Contacts", "Club_Id", "dbo.Clubs");
            DropIndex("dbo.Contacts", new[] { "Club_Id" });
            RenameColumn(table: "dbo.Contacts", name: "Club_Id", newName: "ClubId");
            AlterColumn("dbo.Contacts", "ClubId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Contacts", "ClubId");
            AddForeignKey("dbo.Contacts", "ClubId", "dbo.Clubs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contacts", "ClubId", "dbo.Clubs");
            DropIndex("dbo.Contacts", new[] { "ClubId" });
            AlterColumn("dbo.Contacts", "ClubId", c => c.Guid());
            RenameColumn(table: "dbo.Contacts", name: "ClubId", newName: "Club_Id");
            CreateIndex("dbo.Contacts", "Club_Id");
            AddForeignKey("dbo.Contacts", "Club_Id", "dbo.Clubs", "Id");
        }
    }
}
