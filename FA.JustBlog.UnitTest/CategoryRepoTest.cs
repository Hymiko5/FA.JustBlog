using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;

namespace FA.JustBlog.UnitTest
{
    public class CategoryRepoTest
    {
        private JustBlogContext _context;
        private CategoryRepository _categoryRepository;
        [SetUp]
        public void Setup()
        {
            _context = new JustBlogContext();
            _categoryRepository = new CategoryRepository(_context);
        }

        [Test]
        public void AddCategory_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };

            // Act
            _categoryRepository.AddCategory(category);

            // Assert
            var addedCategory = _categoryRepository.Find(category.Id);
            Assert.NotNull(addedCategory);
            Assert.AreEqual(category.Name, addedCategory.Name);
            Assert.Pass();
        }


        [Test]
        public void UpdateCategory_ShouldUpdateCategoryInDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };
            _categoryRepository.AddCategory(category);

            // Act
            category.Name = "UpdatedCategory";
            _categoryRepository.UpdateCategory(category);

            // Assert
            var updatedCategory = _categoryRepository.Find(category.Id);
            Assert.NotNull(updatedCategory);
            Assert.AreEqual(category.Name, updatedCategory.Name);
        }

        [Test]
        public void DeleteCategory_ShouldDeleteCategoryFromDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };
            _categoryRepository.AddCategory(category);

            // Act
            _categoryRepository.DeleteCategory(category);

            // Assert
            var deletedCategory = _categoryRepository.Find(category.Id);
            Assert.Null(deletedCategory);
        }

        [Test]
        public void GetAllCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var category1 = new Category { Name = "Category1" };
            var category2 = new Category { Name = "Category2" };
            _categoryRepository.AddCategory(category1);
            _categoryRepository.AddCategory(category2);

            // Act
            var categories = _categoryRepository.GetAllCategories();

            // Assert
            Assert.AreEqual(_context.Categories.Count(), categories.Count);
            Assert.IsTrue(categories.Any(c => c.Name == category1.Name));
            Assert.IsTrue(categories.Any(c => c.Name == category2.Name));
        }



    }
}