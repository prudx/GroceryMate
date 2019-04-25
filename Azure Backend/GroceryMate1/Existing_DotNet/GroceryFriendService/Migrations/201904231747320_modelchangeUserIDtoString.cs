namespace GroceryFriendService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelchangeUserIDtoString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Receipts", new[] { "User_Id" });
            DropColumn("dbo.Receipts", "UserId");
            RenameColumn(table: "dbo.Receipts", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Receipts", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Users", "UserId", c => c.String());
            CreateIndex("dbo.Receipts", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Receipts", new[] { "UserId" });
            AlterColumn("dbo.Users", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Receipts", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Receipts", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Receipts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Receipts", "User_Id");
        }
    }
}
