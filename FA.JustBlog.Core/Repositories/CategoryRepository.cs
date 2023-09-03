using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class CategoryRepository:GenericRepository<Category>, ICategoryRepository
    {
        private readonly JustBlogContext _context;
        public CategoryRepository(JustBlogContext context) :base(context)
        {
            _context = context;
        }

        public Category Find(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {

            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            var category = Find(categoryId);
            if (category != null)
            {
                DeleteCategory(category);
            }
        }

        public IList<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
