using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.UnitTest
{
    public class TagRepoTest
    {
        private JustBlogContext _context;
        private IGenericRepository<Tag> _genericRepository;
        private UnitOfWork _unitOfWork;
        private TagRepository _tagRepository;
        [SetUp]
        public async Task Setup()
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
            _genericRepository = new GenericRepository<Tag>(_context);
            _unitOfWork = new UnitOfWork(_context);
            _tagRepository = new TagRepository(_genericRepository, _unitOfWork);
        }

        [Test]
        public async Task AddTag_ShouldAddTagToDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };

            // Act
            await _tagRepository.AddAsync(tag);

            // Assert
            var addedTag = await _tagRepository.GetByIdAsync(tag.Id);
            Assert.NotNull(addedTag);
            Assert.AreEqual(tag.Name, addedTag.Name);
        }

        [Test]
        public async Task UpdateTag_ShouldUpdateTagInDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            await _tagRepository.AddAsync(tag);

            // Act
            tag.Name = "UpdatedTag";
            await _tagRepository.UpdateAsync(tag);

            // Assert
            var updatedTag = await _tagRepository.GetByIdAsync(tag.Id);
            Assert.NotNull(updatedTag);
            Assert.AreEqual(tag.Name, updatedTag.Name);
        }

        [Test]
        public async Task DeleteTag_ShouldDeleteTagFromDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            await _tagRepository.AddAsync(tag);

            // Act
            await _tagRepository.DeleteAsync(tag);

            // Assert
            var deletedTag = await _tagRepository.GetByIdAsync(tag.Id);
            Assert.Null(deletedTag);
        }

        [Test]
        public async Task GetAllTags_ShouldReturnAllTags()
        {
            // Arrange
            var tag1 = new Tag
            {
                Name = "Technology",
                UrlSlug = "technology",
                Description = "Tag related to technology.",
                Count = 0,
                PostTagMaps = new List<PostTagMap>() // Create a list of PostTagMap if needed
            };

            var tag2 = new Tag
            {
                Name = "Programming",
                UrlSlug = "programming",
                Description = "Tag related to programming.",
                Count = 0,
                PostTagMaps = new List<PostTagMap>() // Create a list of PostTagMap if needed
            };

            var tag3 = new Tag
            {
                Name = "Travel",
                UrlSlug = "travel",
                Description = "Tag related to travel.",
                Count = 0,
                PostTagMaps = new List<PostTagMap>() // Create a list of PostTagMap if needed
            };

            await _tagRepository.AddAsync(tag1);
            await _tagRepository.AddAsync(tag2);

            // Act
            var tags = await _tagRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(_context.Tags.Count(), tags.Count());
            Assert.IsTrue(tags.Any(t => t.Name == tag1.Name));
            Assert.IsTrue(tags.Any(t => t.Name == tag2.Name));
            Assert.IsTrue(tags.Any(t => t.Name == tag3.Name));
        }

        [Test]
        public async Task GetTagByUrlSlug_ShouldReturnCorrectTag()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            await _tagRepository.AddAsync(tag);

            // Act
            var retrievedTag = await _tagRepository.GetTagByUrlSlugAsync(tag.UrlSlug);

            // Assert
            Assert.NotNull(retrievedTag);
            Assert.AreEqual(tag.Name, retrievedTag.Name);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
        }
    }
}
