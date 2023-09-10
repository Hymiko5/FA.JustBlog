using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FA.JustBlog.UnitTest
{
    public class PostRepoTest
    {
        private JustBlogContext _context;
        private IGenericRepository<Post> _genericRepository;
        private UnitOfWork _unitOfWork;
        private PostRepository _postRepository;
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
            _genericRepository = new GenericRepository<Post>(_context);
            _unitOfWork = new UnitOfWork(_context);
            _postRepository = new PostRepository(_genericRepository, _unitOfWork);
        }

        [Test]
        public async Task AddPost_ShouldAddPostToDatabase()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test Post",
                ShortDescription = "Test Description",
                PostContent = "Test Content",
                UrlSlug = "test-post",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 1,
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            // Act
            await _postRepository.AddAsync(post);

            // Assert
            var addedPost = await _postRepository.GetByIdAsync(post.Id);
            Assert.NotNull(addedPost);
            Assert.AreEqual(post.Title, addedPost.Title);
        }

        [Test]
        public async Task UpdatePost_ShouldUpdatePostInDatabase()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test Post",
                ShortDescription = "Test Description",
                PostContent = "Test Content",
                UrlSlug = "test-post",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 1,
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            await _postRepository.AddAsync(post);
            // Act
            post.Title = "Updated Post";
            await _postRepository.UpdateAsync(post);

            // Assert
            var updatedPost = await _postRepository.GetByIdAsync(post.Id);
            Assert.NotNull(updatedPost);
            Assert.AreEqual(post.Title, updatedPost.Title);
        }

        [Test]
        public async Task DeletePost_ShouldDeletePostFromDatabase()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test Post",
                ShortDescription = "Test Description",
                PostContent = "Test Content",
                UrlSlug = "test-post",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 1,
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            await _postRepository.AddAsync(post);

            // Act
            await _postRepository.DeleteAsync(post);

            // Assert
            var deletedPost = await _postRepository.GetByIdAsync(post.Id);
            Assert.Null(deletedPost);
        }

        [Test]
        public async Task GetPublishedPosts_ShouldReturnPublishedPosts()
        {
            // Arrange
            var post1 = new Post
            {
                Title = "Sample Post 1",
                ShortDescription = "This is a short description for Sample Post 1.",
                PostContent = "Content for Sample Post 1.",
                UrlSlug = "sample-post-1",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 1, // Replace with an actual category ID
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            var post2 = new Post
            {
                Title = "Sample Post 2",
                ShortDescription = "This is a short description for Sample Post 2.",
                PostContent = "Content for Sample Post 2.",
                UrlSlug = "sample-post-2",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 2, // Replace with an actual category ID
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            var post3 = new Post
            {
                Title = "Sample Post 3",
                ShortDescription = "This is a short description for Sample Post 3.",
                PostContent = "Content for Sample Post 3.",
                UrlSlug = "sample-post-3",
                Published = true,
                PostedOn = DateTime.Now,
                Modified = false,
                CategoryId = 1, // Replace with an actual category ID
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            await _postRepository.AddAsync(post1);
            await _postRepository.AddAsync(post2);
            await _postRepository.AddAsync(post3);

            // Act
            var publishedPosts = await _postRepository.GetPublishedPostsAsync();

            // Assert
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post1.Title));
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post2.Title));
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post2.Title));
        }

        [Test]
        public async Task GetUnpublishedPosts_ShouldReturnUnpublishedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {  Published = true, Title = "Title 1" },
            new Post {  Published = false, Title = "Title 1" },
            new Post { Published = false, Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);
            // Act
            var result = await _postRepository.GetUnpublishedPostsAsync();

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result.Any(p => p.Published));
        }

        [Test]
        public async Task GetLatestPosts_ShouldReturnLatestPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 4, Published = true, PostedOn = DateTime.Now.AddDays(-1), Title = "Title 1" },
            new Post { Id = 5, Published = true, PostedOn = DateTime.Now.AddDays(-2), Title = "Title 1" },
            new Post { Id = 6, Published = true, PostedOn = DateTime.Now.AddDays(-3) , Title = "Title 1"}
        };

            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);
            // Act
            var result = await _postRepository.GetLatestPostsAsync(2);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(4, result.ToArray()[0].Id); // Assuming the ordering is descending by PostedOn
            Assert.AreEqual(5, result.ToArray()[1].Id);
        }

        [Test]
        public async Task GetPostsByMonth_ShouldReturnPostsForGivenMonth()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {  Published = true, PostedOn = new DateTime(2023, 8, 15), Title = "Title 1" },
            new Post { Published = true, PostedOn = new DateTime(2023, 8, 20), Title = "Title 1" },
            new Post {  Published = true, PostedOn = new DateTime(2023, 9, 5), Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.GetPostsByMonthAsync(new DateTime(2023, 8, 1));

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.PostedOn.Month == 8));
        }

        [Test]
        public async Task CountPostsForCategory_ShouldReturnCountOfPostsForCategory()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 6, Published = true, Category = new Category { Name = "CategoryA" }, Title = "Title1" },
            new Post { Id = 7, Published = true, Category = new Category { Name = "CategoryA" }, Title = "Title1" },
            new Post { Id = 8, Published = true, Category = new Category { Name = "CategoryB" }, Title = "Title1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.CountPostsForCategoryAsync("CategoryA");

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task GetPostsByCategory_ShouldReturnPostsForCategory()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {Published = true, Category = new Category { Name = "CategoryA" } , Title = "Title 1"},
            new Post {Published = true, Category = new Category { Name = "CategoryA" }, Title = "Title 1" },
            new Post {Published = true, Category = new Category { Name = "CategoryB" }, Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.GetPostsByCategoryAsync("CategoryA");

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.Category.Name == "CategoryA"));
        }

        [Test]
        public async Task CountPostsForTag_ShouldReturnCountOfPostsForTag()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } }, Title = "Title 1" },
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } }, Title = "Title 1" },
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagB" } } }, Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.CountPostsForTagAsync("TagA");

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public async Task GetPostsByTag_ShouldReturnPostsForTag()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } }, Title = "Title 1" },
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } }, Title = "Title 1" },
            new Post {Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagB" } } }, Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.GetPostsByTagAsync("TagA");

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.PostTagMaps.Any(pt => pt.Tag.Name == "TagA")));
        }

        [Test]
        public async Task GetMostViewedPost_ShouldReturnMostViewedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 4, Published = true, ViewCount = 100, Title = "Title 1" },
            new Post { Id = 5, Published = true, ViewCount = 200, Title = "Title 1" },
            new Post { Id = 6, Published = true, ViewCount = 50, Title = "Title 1" }
            };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.GetMostViewedPostAsync(2);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(5, result.ToArray()[0].Id); // Assuming the ordering is descending by ViewCount
            Assert.AreEqual(4, result.ToArray()[1].Id);
        }

        [Test]
        public async Task GetHighestPosts_ShouldReturnHighestRatedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post {Id = 4, Published = true, RateCount = 5, TotalRate = 25, Title = "Title 1" },
            new Post {Id = 5, Published = true, RateCount = 3, TotalRate = 14, Title = "Title 1" },
            new Post {Id = 6, Published = true, RateCount = 4, TotalRate = 20, Title = "Title 1" }
        };
            await _postRepository.AddAsync(testData[0]);
            await _postRepository.AddAsync(testData[1]);
            await _postRepository.AddAsync(testData[2]);

            // Act
            var result = await _postRepository.GetHighestPostsAsync(2);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(4, result.ToArray()[0].Id); // Assuming the ordering is descending by Rate
            Assert.AreEqual(6, result.ToArray()[1].Id);
        }
        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
        }
    }
}
