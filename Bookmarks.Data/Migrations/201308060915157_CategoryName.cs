namespace Bookmarks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", "IX_Name");
            CreateIndex("dbo.Categories", "Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", "IX_Name");
            CreateIndex("dbo.Categories", "Name", unique:true);
        }
    }
}
