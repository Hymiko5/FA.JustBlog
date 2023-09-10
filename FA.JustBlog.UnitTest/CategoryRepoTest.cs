using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FA.JustBlog.UnitTest
{
    public class CategoryRepoTest
    {
        private JustBlogContext _context;
        private IGenericRepository<Category> _genericRepository;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;

        [SetUp]
        public void Setup()
        {
            // Create an options builder for an in-memory database
            var options = new DbContextOptionsBuilder<JustBlogContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            // Create an instance of the in-memory database context
            _context = new JustBlogContext(options);

            // Initialize the database with test data
            _context.Database.EnsureCreated();
            _genericRepository = new GenericRepository<Category>(_context);
            _unitOfWork = new UnitOfWork(_context);
            _categoryRepository = new CategoryRepository(_unitOfWork, _genericRepository);
        }

        [Test]
        public async Task AddCategory_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };

            // Act
            _categoryRepository.AddAsync(category);

            // Assert
            var addedCategory = await _categoryRepository.GetByIdAsync(category.Id);
            Assert.NotNull(addedCategory);
            Assert.AreEqual(category.Name, addedCategory.Name);
            Assert.Pass();
        }


        [Test]
        public async Task UpdateCategory_ShouldUpdateCategoryInDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };
            await _categoryRepository.AddAsync(category);

            // Act
            category.Name = "UpdatedCategory";
            await _categoryRepository.UpdateAsync(category);

            // Assert
            var updatedCategory = await _categoryRepository.GetByIdAsync(category.Id);
            Assert.NotNull(updatedCategory);
            Assert.AreEqual(category.Name, updatedCategory.Name);
        }

        [Test]
        public async Task DeleteCategory_ShouldDeleteCategoryFromDatabase()
        {
            // Arrange
            var category = new Category
            {
                Name = "TestCategory",
                UrlSlug = "test-category",
                Description = "Test Description"
            };
            await _categoryRepository.AddAsync(category);

            // Act
            await _categoryRepository.DeleteAsync(category);

            // Assert
            var deletedCategory = await _categoryRepository.GetByIdAsync(category.Id);
            Assert.Null(deletedCategory);
        }

        [Test]
        public async Task GetAllCategories_ShouldReturnAllCategories()
        {
            // Arrange
            var category1 = new Category { Name = "Category1" };
            var category2 = new Category { Name = "Category2" };
            await _categoryRepository.AddAsync(category1);
            await _categoryRepository.AddAsync(category2);

            // Act
            var categories = await _categoryRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(_context.Categories.Count(), categories.Count());
            Assert.IsTrue(categories.Any(c => c.Name == category1.Name));
            Assert.IsTrue(categories.Any(c => c.Name == category2.Name));
        }
        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
        }


    }
}