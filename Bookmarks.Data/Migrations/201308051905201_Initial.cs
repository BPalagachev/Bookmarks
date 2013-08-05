namespace Bookmarks.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Bookmarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(maxLength: 256),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 300),
                        LastEdit = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        TimesClicked = c.Long(nullable: false),
                        User_UserId = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.User_UserId)
                .Index(t => t.Category_Id)
                .Index(t=>t.Title)
                .Index(t=>t.Url);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 75),
                        IsDeleted = c.Boolean(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.User_UserId)
                .Index(t => t.User_UserId)
                .Index(t => t.Name, unique:true);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 75),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagBookmarks",
                c => new
                    {
                        Tag_Id = c.Int(nullable: false),
                        Bookmark_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Bookmark_Id })
                .ForeignKey("dbo.Tags", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bookmarks", t => t.Bookmark_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Bookmark_Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TagBookmarks", new[] { "Bookmark_Id" });
            DropIndex("dbo.TagBookmarks", new[] { "Tag_Id" });
            DropIndex("dbo.Categories", new[] { "User_UserId" });
            DropIndex("dbo.Bookmarks", new[] { "Category_Id" });
            DropIndex("dbo.Bookmarks", new[] { "User_UserId" });
            DropForeignKey("dbo.TagBookmarks", "Bookmark_Id", "dbo.Bookmarks");
            DropForeignKey("dbo.TagBookmarks", "Tag_Id", "dbo.Tags");
            DropForeignKey("dbo.Categories", "User_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Bookmarks", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Bookmarks", "User_UserId", "dbo.UserProfile");
            DropTable("dbo.TagBookmarks");
            DropTable("dbo.Tags");
            DropTable("dbo.Categories");
            DropTable("dbo.Bookmarks");
            DropTable("dbo.UserProfile");
        }
    }
}
