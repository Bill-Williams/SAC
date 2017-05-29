namespace SAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetUsers2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserClaims", "AspNetUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "AspNetUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserClaims", new[] { "AspNetUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "AspNetUser_Id" });
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropColumn("dbo.AspNetUserLogins", "AspNetUser_Id");
            DropColumn("dbo.AspNetUserClaims", "AspNetUser_Id");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropPrimaryKey("dbo.AspNetUserLogins");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddColumn("dbo.AspNetUserLogins", "AspNetUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUserClaims", "AspNetUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUserLogins", "AspNetUser_Id");
            CreateIndex("dbo.AspNetUserClaims", "AspNetUser_Id");
            AddForeignKey("dbo.AspNetUserLogins", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUserClaims", "AspNetUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
