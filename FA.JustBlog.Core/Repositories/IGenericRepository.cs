using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        void Update(T entity);
        Task DeleteAsync(int id);
        void Delete(T entity);
        Task<int> CountAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> GetAllIncludeAsync(params string[] includeProps);
        Task<IEnumerable<T>> GetIncludeAsync(Expression<Func<T, bool>> filter, params string[] includeProps);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
        Task<T> FirstOrDefaultIncludeAsync(Expression<Func<T, bool>> filter, params string[] includeProps);
    }
}
