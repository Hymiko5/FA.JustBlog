using FA.JustBlog.Core.Models;

namespace JustBlog.MVC.Models
{
    public class CategoryModel
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
        /// Category description.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// List Post.
        /// </summary>
        public IList<Post>? Posts { get; set; }
    }
}
