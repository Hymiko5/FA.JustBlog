using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class Category:IEntity
    {
        /// <summary>
        /// The unique id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Category name.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Url Slug.
        /// </summary>
        public string? UrlSlug { get; set; }
        /// <summary>
        /// Category description.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// List Post.
        /// </summary>
        public IList<Post>? Posts { get; set; }
    }
}
