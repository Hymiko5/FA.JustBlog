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
        int Add(T entity);
        T GetById(object id);
        T Update(T entity);
        int Delete(int id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetAllInclude(params string[] includeProps);
        IEnumerable<T> GetInclude(Expression<Func<T, bool>> filter, params string[] includeProps);

        T FirstOrDefault(Expression<Func<T, bool>> filter);
        T FirstOrDefaultInclude(Expression<Func<T, bool>> filter, params string[] includeProps);
    }
}
