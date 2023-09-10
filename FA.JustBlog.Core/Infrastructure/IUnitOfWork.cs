using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Infrastructure
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> CategoryRepository { get; }

        IGenericRepository<Tag> TagRepository { get; }

        IGenericRepository<Post> PostRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        int Commit();
        Task<int> CommitAsync();
        IGenericRepository<T> GenericRepository<T>() where T : class, IEntity;
    }

    
}
