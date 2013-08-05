using Bookmarks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarks.Web.ViewModels
{
    public class DisplayBookmarkViewModel
    {
        public DisplayBookmarkViewModel(Bookmark bookmark)
        {
            this.Id = bookmark.Id;
            this.Url = bookmark.Url;
            this.Title = bookmark.Title;
            this.Description = bookmark.Description;
            this.UserName = bookmark.User.UserName;
            this.LastEdit = bookmark.LastEdit;
            this.Category = bookmark.Category.Name;
        }

        public DisplayBookmarkViewModel()
        {
        }
        
        public int Id { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public string UserName { get; set; }

        public DateTime LastEdit { get; set; }

        public string Category { get; set; }
    }
}