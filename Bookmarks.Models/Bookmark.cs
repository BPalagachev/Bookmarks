using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookmarks.Models
{
    public class Bookmark
    {
        private ICollection<Tag> tags;

        public Bookmark()
        {
            this.tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public UserProfile User { get; set; }

        public DateTime LastEdit { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Tag> Tags
        {
            get { return this.tags; }
            set { this.tags = value; }
        }

        public bool IsDeleted { get; set; }

        public long TimesClicked { get; set; }
    }
}
