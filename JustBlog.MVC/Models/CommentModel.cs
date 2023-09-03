using FA.JustBlog.Core.Models;

namespace JustBlog.MVC.Models
{
    public class CommentModel
    {
        public int PostId { get; set; }
        public string CommentName { get; set; }
        public string CommentEmail { get; set; }
        public string CommentTitle { get; set; }
        public string CommentBody { get; set; }
    }
}
