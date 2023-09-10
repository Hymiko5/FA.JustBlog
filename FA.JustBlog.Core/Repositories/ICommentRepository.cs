using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public interface ICommentRepository:IBaseService<Comment>
    {
        Task AddCommentAsync(int postId, string commentName, string commentEmail, string commentTitle, string commentBody);
        Task<IEnumerable<Comment>> GetCommentsForPostAsync(int? postId);
        Task<IEnumerable<Comment>> GetCommentsForPostAsync(Post post);
    }
}
