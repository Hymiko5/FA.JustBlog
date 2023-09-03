using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
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
        private TagRepository _tagRepository;

        [SetUp]
        public void Setup()
        {
            _context = new JustBlogContext();
            _tagRepository = new TagRepository(_context);
        }

        [Test]
        public void AddTag_ShouldAddTagToDatabase()
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
            _tagRepository.AddTag(tag);

            // Assert
            var addedTag = _tagRepository.Find(tag.Id);
            Assert.NotNull(addedTag);
            Assert.AreEqual(tag.Name, addedTag.Name);
        }

        [Test]
        public void UpdateTag_ShouldUpdateTagInDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            _tagRepository.AddTag(tag);

            // Act
            tag.Name = "UpdatedTag";
            _tagRepository.UpdateTag(tag);

            // Assert
            var updatedTag = _tagRepository.Find(tag.Id);
            Assert.NotNull(updatedTag);
            Assert.AreEqual(tag.Name, updatedTag.Name);
        }

        [Test]
        public void DeleteTag_ShouldDeleteTagFromDatabase()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            _tagRepository.AddTag(tag);

            // Act
            _tagRepository.DeleteTag(tag);

            // Assert
            var deletedTag = _tagRepository.Find(tag.Id);
            Assert.Null(deletedTag);
        }

        [Test]
        public void GetAllTags_ShouldReturnAllTags()
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

            _tagRepository.AddTag(tag1);
            _tagRepository.AddTag(tag2);

            // Act
            var tags = _tagRepository.GetAllTags();

            // Assert
            Assert.AreEqual(_context.Tags.Count(), tags.Count);
            Assert.IsTrue(tags.Any(t => t.Name == tag1.Name));
            Assert.IsTrue(tags.Any(t => t.Name == tag2.Name));
            Assert.IsTrue(tags.Any(t => t.Name == tag3.Name));
        }

        [Test]
        public void GetTagByUrlSlug_ShouldReturnCorrectTag()
        {
            // Arrange
            var tag = new Tag
            {
                Name = "TestTag",
                UrlSlug = "test-tag",
                Description = "Test Description",
                Count = 0
            };
            _tagRepository.AddTag(tag);

            // Act
            var retrievedTag = _tagRepository.GetTagByUrlSlug(tag.UrlSlug);

            // Assert
            Assert.NotNull(retrievedTag);
            Assert.AreEqual(tag.Name, retrievedTag.Name);
        }
    }
}
