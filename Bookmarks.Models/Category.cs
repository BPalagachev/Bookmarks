using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarks.Models
{
    public class Category
    {
        private ICollection<Bookmark> booksmarks;

        public Category()
        {
            this.booksmarks = new HashSet<Bookmark>();
        }

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings=false), MaxLength(75)]
        public string Name { get; set; }

        public UserProfile User { get; set; }

        public virtual ICollection<Bookmark> Bookmarks
        {
            get { return this.booksmarks; }
            set { this.booksmarks = value; }
        }

        public bool IsDeleted { get; set; }
    }
}
