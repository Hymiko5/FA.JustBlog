using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FA.JustBlog.Core.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly JustBlogContext _context;
        public CommentRepository(JustBlogContext context) :base(context)
        {
            _context = context;
        }
        public Comment Find(int commentId)
        {
            return _context.Comments.FirstOrDefault(comment => comment.Id == commentId);
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void AddComment(int postId, string commentName, string commentEmail, string commentTitle, string commentBody)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if (post != null)
            {
                var newComment = new Comment
                {
                    Post = post,
                    Name = commentName,
                    Email = commentEmail,
                    CommentHeader = commentTitle,
                    CommentText = commentBody,
                    CommentTime = DateTime.Now
                };
                _context.Comments.Add(newComment);
                _context.SaveChanges();
            }
        }

        public void UpdateComment(Comment comment)
        {
            // Assuming you have a method to update an entity in your context
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = Find(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }

        public IList<Comment> GetAllComments()
        {
            return _context.Comments.ToList();
        }

        public IList<Comment> GetCommentsForPost(int? postId)
        {
            return _context.Comments.Where(comment => comment.Post != null && comment.Post.Id == postId).ToList();
        }

        public IList<Comment> GetCommentsForPost(Post post)
        {
            return _context.Comments.Where(comment => comment.Post == post).ToList();
        }

    }
}
