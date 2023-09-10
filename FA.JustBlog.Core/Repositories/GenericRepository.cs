using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private JustBlogContext context;
        private readonly DbSet<T> dbSet;

        public GenericRepository(JustBlogContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public async virtual Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            dbSet.Remove(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual async Task<T?> FirstOrDefaultIncludeAsync(Expression<Func<T, bool>> filter, params string[] includeProps)
        {
            var query = dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async virtual Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async virtual Task<IEnumerable<T>> GetAllIncludeAsync(params string[] includeProps)
        {
            var query = dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetIncludeAsync(Expression<Func<T, bool>> filter, params string[] includeProps)
        {

            var query = dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return await query.Where(filter).ToListAsync();
        }

        public async virtual Task AddAsync(T entity)
        {
            var result = await dbSet.AddAsync(entity);
            Console.WriteLine(result);
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.CountAsync(filter);
        }
    }
}