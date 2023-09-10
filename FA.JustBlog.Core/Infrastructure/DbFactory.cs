using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        JustBlogContext _dbContext;
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public JustBlogContext Init() => _dbContext ?? (_dbContext = new JustBlogContext());

        protected override void DisposeCore()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }
    }
}
