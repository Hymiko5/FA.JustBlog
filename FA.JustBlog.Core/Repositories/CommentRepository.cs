using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FA.JustBlog.Core.Repositories
{
    public class CommentRepository : BaseService<Comment>, ICommentRepository
    {
        private readonly IGenericRepository<Comment> commentRepository;

        public CommentRepository(IUnitOfWork unitOfWork, IGenericRepository<Comment> commentRepository) :base(unitOfWork, commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task AddCommentAsync(int postId, string commentName, string commentEmail, string commentTitle, string commentBody)
        {
            var post = await unitOfWork.PostRepository.GetByIdAsync(postId);
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
                
                await commentRepository.AddAsync(newComment);
                await unitOfWork.CommitAsync();
            }
        }


        public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(int? postId)
        {
            return await commentRepository.FindAsync(comment => comment.Post != null && comment.Post.Id == postId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsForPostAsync(Post post)
        {
            return await commentRepository.FindAsync(comment => comment.Post == post);
        }

    }
}
