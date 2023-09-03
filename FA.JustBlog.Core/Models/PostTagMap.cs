using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class PostTagMap
    {
        /// <summary>
        /// Post Id.
        /// </summary>
        public int PostId { get; set; }
        /// <summary>
        /// Post.
        /// </summary>
        public Post? Post { get; set; }
        /// <summary>
        /// Tag Id.
        /// </summary>
        public int TagId { get; set; }
        /// <summary>
        /// Tag.
        /// </summary>
        public Tag? Tag { get; set; }
    }
}
