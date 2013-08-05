using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
//using System.Web.Security;
using Bookmarks.Models;

namespace Bookmarks.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("BooksmarksDb")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
