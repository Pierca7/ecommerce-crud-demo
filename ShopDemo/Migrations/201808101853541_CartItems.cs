namespace ShopDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "Cart_CartID", c => c.Int());
            CreateIndex("dbo.CartItems", "Cart_CartID");
            AddForeignKey("dbo.CartItems", "Cart_CartID", "dbo.Carts", "CartID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "Cart_CartID", "dbo.Carts");
            DropIndex("dbo.CartItems", new[] { "Cart_CartID" });
            DropColumn("dbo.CartItems", "Cart_CartID");
        }
    }
}
