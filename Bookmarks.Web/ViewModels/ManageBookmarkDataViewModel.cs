using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Bookmarks.Web.ViewModels
{
    public class ManageBookmarkDataViewModel
    {
        public ManageBookmarkDataViewModel()
        {
        }

        public ManageBookmarkDataViewModel(Bookmarks.Models.Bookmark bookmark)
        {
            this.Id = bookmark.Id;
            this.Url = bookmark.Url;
            this.Title = bookmark.Title;
            this.Description = bookmark.Title;
            this.Category = bookmark.Category.Name;
        }

        public int Id { get; set; }

        [MaxLength(256), Required]
        public string Url { get; set; }

        [MaxLength(100, ErrorMessage="Title length cannot exceed 100 symbols")]
        [MinLength(3, ErrorMessage="Title length cannot be less then 100 symbols")]
        public string Title { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }
    }
}