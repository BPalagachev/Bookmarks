using Bookmarks.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarks.Data
{
    public class BookmarksContext : DbContext
    {
        public BookmarksContext()
            : base("BooksmarksDb")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
