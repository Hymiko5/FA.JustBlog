using System.Linq.Expressions;

namespace FA.JustBlog.Core.BaseServices
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        Task<int> DeleteAsync(int id);
        Task DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity?> FirstOrDefaultIncludeAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProps);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllIncludeAsync(params string[] includeProps);
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetIncludeAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProps);
        Task UpdateAsync(TEntity entity);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);
    }
}