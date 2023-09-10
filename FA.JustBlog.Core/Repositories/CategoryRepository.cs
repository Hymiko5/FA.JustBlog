using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class CategoryRepository:BaseService<Category>, IBaseService<Category>
    {
        public CategoryRepository(IUnitOfWork unitOfWork,IGenericRepository<Category> categoryRepository) :base(unitOfWork, categoryRepository)
        {
            
        }
    }
}
