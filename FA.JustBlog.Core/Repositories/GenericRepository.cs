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
        private readonly JustBlogContext _context;

        private DbSet<T> _dbSet;

        public GenericRepository(JustBlogContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public virtual IEnumerable<T> All()
        {
            return _dbSet.AsNoTracking();
        }

        public virtual int Delete(int id)
        {
            var entity = GetById(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public virtual T FirstOrDefaultInclude(Expression<Func<T, bool>> filter, params string[] includeProps)
        {
            var query = _dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault(filter);
        }

        public virtual T GetById(object id)
        {
            return _dbSet.Find(id) ?? throw new NullReferenceException($"Cannot find entity id: {id}");
        }

        public virtual IEnumerable<T> GetAllInclude(params string[] includeProps)
        {
            var query = _dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public virtual IEnumerable<T> GetInclude(Expression<Func<T, bool>> filter, params string[] includeProps)
        {

            var query = _dbSet.AsQueryable();
            if (includeProps.Length > 0)
            {
                foreach (var prop in includeProps)
                {
                    query = query.Include(prop);
                }
            }
            return query.Where(filter).ToList();
        }

        public virtual int Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public virtual T Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }
    }
}
