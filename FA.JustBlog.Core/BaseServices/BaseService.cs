using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.BaseServices
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IGenericRepository<TEntity> repository;
        public BaseService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public virtual async Task<int> DeleteAsync(int id)
        {
            await repository.DeleteAsync(id);
            return await unitOfWork.CommitAsync();
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            repository.Delete(entity);
            await unitOfWork.CommitAsync();
        }
        public async virtual Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await repository.FindAsync(predicate);
        }
        public async virtual Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await repository.FirstOrDefaultAsync(filter) ?? throw new ArgumentException("Not Found");
        }

        public async virtual Task<TEntity?> FirstOrDefaultIncludeAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProps)
        {
            return await repository.FirstOrDefaultIncludeAsync(filter, includeProps);
        }

        public async virtual Task<TEntity?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async virtual Task<IEnumerable<TEntity>> GetAllIncludeAsync(params string[] includeProps)
        {
            return await repository.GetAllIncludeAsync(includeProps);
        }

        public async virtual Task<IEnumerable<TEntity>> GetIncludeAsync(Expression<Func<TEntity, bool>> filter, params string[] includeProps)
        {
            return await repository.GetIncludeAsync(filter, includeProps);

        }

        public async virtual Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Can not add null value");
            }
            await repository.AddAsync(entity);
            await unitOfWork.CommitAsync();
        }

        public async virtual Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Can not add null value");
            }
            repository.Update(entity);
            await unitOfWork.CommitAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await repository.CountAsync(filter);
        }
    }
}
