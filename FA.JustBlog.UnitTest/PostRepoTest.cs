using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
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
        private Mock<JustBlogContext> _contextMock;
        private PostRepository _postRepositoryMock;
        private PostRepository _postRepository;

        [SetUp]
        public void Setup()
        {
            // Initialize and configure your DbContext mock
            _contextMock = new Mock<JustBlogContext>();

            // Initialize the repository with the mock context
            _postRepositoryMock = new PostRepository(_contextMock.Object);
            _context = new JustBlogContext();
            _postRepository = new PostRepository(_context);
        }

        [Test]
        public void AddPost_ShouldAddPostToDatabase()
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
                Category = new Category(),
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            // Act
            _postRepository.AddPost(post);

            // Assert
            var addedPost = _postRepository.FindPost(post.Id);
            Assert.NotNull(addedPost);
            Assert.AreEqual(post.Title, addedPost.Title);
        }

        [Test]
        public void UpdatePost_ShouldUpdatePostInDatabase()
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
                Category = new Category(),
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            _postRepository.AddPost(post);
            // Act
            post.Title = "Updated Post";
            _postRepository.UpdatePost(post);

            // Assert
            var updatedPost = _postRepository.FirstOrDefault(p => p.Id == post.Id);
            Assert.NotNull(updatedPost);
            Assert.AreEqual(post.Title, updatedPost.Title);
        }

        [Test]
        public void DeletePost_ShouldDeletePostFromDatabase()
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
                Category = new Category(),
                PostTagMaps = new List<PostTagMap>(),
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            _postRepository.AddPost(post);

            // Act
            _postRepository.DeletePost(post);

            // Assert
            var deletedPost = _postRepository.FindPost(post.Id);
            Assert.Null(deletedPost);
        }

        [Test]
        public void GetPublishedPosts_ShouldReturnPublishedPosts()
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
                Category = new Category(), // Create a Category instance
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
                Category = new Category(), // Create a Category instance
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
                Category = new Category(), // Create a Category instance
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };

            _postRepository.AddPost(post1);
            _postRepository.AddPost(post2);
            _postRepository.AddPost(post3);

            // Act
            var publishedPosts = _postRepository.GetPublishedPosts();

            // Assert
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post1.Title));
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post2.Title));
            Assert.IsTrue(publishedPosts.Any(p => p.Title == post2.Title));
        }

        [Test]
        public void GetUnpublishedPosts_ShouldReturnUnpublishedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true },
            new Post { Id = 2, Published = false },
            new Post { Id = 3, Published = false }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetUnpublishedPosts();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsFalse(result.Any(p => p.Published));
        }

        [Test]
        public void GetLatestPosts_ShouldReturnLatestPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, PostedOn = DateTime.Now.AddDays(-1) },
            new Post { Id = 2, Published = true, PostedOn = DateTime.Now.AddDays(-2) },
            new Post { Id = 3, Published = true, PostedOn = DateTime.Now.AddDays(-3) }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetLatestPosts(2);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(1, result[0].Id); // Assuming the ordering is descending by PostedOn
            Assert.AreEqual(2, result[1].Id);
        }

        [Test]
        public void GetPostsByMonth_ShouldReturnPostsForGivenMonth()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, PostedOn = new DateTime(2023, 8, 15) },
            new Post { Id = 2, Published = true, PostedOn = new DateTime(2023, 8, 20) },
            new Post { Id = 3, Published = true, PostedOn = new DateTime(2023, 9, 5) }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepository.GetPostsByMonth(new DateTime(2023, 8, 1));

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.PostedOn.Month == 8));
        }

        [Test]
        public void CountPostsForCategory_ShouldReturnCountOfPostsForCategory()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, Category = new Category { Name = "CategoryA" } },
            new Post { Id = 2, Published = true, Category = new Category { Name = "CategoryA" } },
            new Post { Id = 3, Published = true, Category = new Category { Name = "CategoryB" } }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.CountPostsForCategory("CategoryA");

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetPostsByCategory_ShouldReturnPostsForCategory()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, Category = new Category { Name = "CategoryA" } },
            new Post { Id = 2, Published = true, Category = new Category { Name = "CategoryA" } },
            new Post { Id = 3, Published = true, Category = new Category { Name = "CategoryB" } }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetPostsByCategory("CategoryA");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.Category.Name == "CategoryA"));
        }

        [Test]
        public void CountPostsForTag_ShouldReturnCountOfPostsForTag()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } } },
            new Post { Id = 2, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } } },
            new Post { Id = 3, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagB" } } } }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.CountPostsForTag("TagA");

            // Assert
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetPostsByTag_ShouldReturnPostsForTag()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } } },
            new Post { Id = 2, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagA" } } } },
            new Post { Id = 3, Published = true, PostTagMaps = new List<PostTagMap> { new PostTagMap { Tag = new Tag { Name = "TagB" } } } }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetPostsByTag("TagA");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.IsTrue(result.All(p => p.PostTagMaps.Any(pt => pt.Tag.Name == "TagA")));
        }

        [Test]
        public void GetMostViewedPost_ShouldReturnMostViewedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, ViewCount = 100 },
            new Post { Id = 2, Published = true, ViewCount = 200 },
            new Post { Id = 3, Published = true, ViewCount = 50 }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetMostViewedPost(2);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(2, result[0].Id); // Assuming the ordering is descending by ViewCount
            Assert.AreEqual(1, result[1].Id);
        }

        [Test]
        public void GetHighestPosts_ShouldReturnHighestRatedPosts()
        {
            // Arrange
            var testData = new List<Post>
        {
            new Post { Id = 1, Published = true, RateCount = 5, TotalRate = 25 },
            new Post { Id = 2, Published = true, RateCount = 3, TotalRate = 14 },
            new Post { Id = 3, Published = true, RateCount = 4, TotalRate = 20 }
        }.AsQueryable();
            Mock<DbSet<Post>> mockDbSet = new Mock<DbSet<Post>>();
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Posts).Returns(mockDbSet.Object);

            // Act
            var result = _postRepositoryMock.GetHighestPosts(2);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.Published));
            Assert.AreEqual(1, result[0].Id); // Assuming the ordering is descending by Rate
            Assert.AreEqual(3, result[1].Id);
        }

    }
}
