using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bookmarks.Web.ViewModels
{
    public class CategoriesDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Number of Bookmarks")]
        public int NumberOfBookmarks { get; set; }

        public bool IsDeleted { get; set; }
    }
}