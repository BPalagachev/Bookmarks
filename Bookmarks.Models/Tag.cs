using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarks.Models
{
    public class Tag
    {
        private ICollection<Bookmark> booksmarks;

        public Tag()
        {
            this.booksmarks = new HashSet<Bookmark>();
        }

        [Key]
        public int Id { get; set; }
                
        //[Key,Column(Order = 1),  MaxLength(75)]
        [MaxLength(75)]
        public string Name { get; set; }

        public virtual ICollection<Bookmark> Bookmarks
        {
            get { return this.booksmarks; }
            set { this.booksmarks = value; }
        }
    }
}
