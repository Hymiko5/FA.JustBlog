using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JustBlogContext context;

        public JustBlogContext DataContext => context;

        private IGenericRepository<Category> _categoryRepository;

        public IGenericRepository<Category> CategoryRepository =>
            _categoryRepository ?? new GenericRepository<Category>(context);

        private IGenericRepository<Tag> _tagRepository;

        public IGenericRepository<Tag> TagRepository =>
            _tagRepository ?? new GenericRepository<Tag>(context);

        private IGenericRepository<Post> _postRepository;

        public IGenericRepository<Post> PostRepository =>
            _postRepository ?? new GenericRepository<Post>(context);

        private IGenericRepository<Comment> _commentRepository;

        public IGenericRepository<Comment> CommentRepository =>
            _commentRepository ?? new GenericRepository<Comment>(context);

        public UnitOfWork(JustBlogContext context)
        {
            this.context = context;
        }
        public int Commit()
        {
            return context.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class, IEntity
        {
            return new GenericRepository<T>(context);
        }
    }
}
