using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.UnitTest
{
    public class CommentRepoTest
    {
        private JustBlogContext _context;
        private ICommentRepository _commentRepository;
        private Mock<JustBlogContext> _contextMock;
        private ICommentRepository _commentRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _context = new JustBlogContext();
            _commentRepository = new CommentRepository(_context);
            // Initialize and configure your DbContext mock
            _contextMock = new Mock<JustBlogContext>();

            // Initialize the repository with the mock context
            _commentRepositoryMock = new CommentRepository(_contextMock.Object);
        }

        [Test]
        public void AddComment_ShouldAddCommentToDatabase()
        {
            // Arrange
            var comment = new Comment
            {
                Name = "Test Commenter",
                Email = "test@example.com",
                CommentHeader = "Test Header",
                CommentText = "Test Comment",
                CommentTime = DateTime.Now
            };

            // Act
            _commentRepository.AddComment(comment);

            // Assert
            var addedComment = _commentRepository.Find(comment.Id);
            Assert.NotNull(addedComment);
            Assert.AreEqual(comment.Name, addedComment.Name);
        }

        [Test]
        public void AddCommentWithPostId_ShouldAddCommentWithAssociatedPost()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test comment Post 1",
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
            _context.Posts.Add(post);
            _context.SaveChanges();

            // Act
            _commentRepository.AddComment(post.Id, "Test Commenter", "test@example.com", "Test Header", "Test Comment");

            // Assert
            var addedComment = _commentRepository.GetCommentsForPost(post.Id).FirstOrDefault();
            Assert.NotNull(addedComment);
            Assert.AreEqual(post.Id, addedComment.Post.Id);
        }

        [Test]
        public void UpdateComment_ShouldUpdateCommentInDatabase()
        {
            // Arrange
            var comment = new Comment
            {
                Name = "Test Commenter",
                Email = "test@example.com",
                CommentHeader = "Test Header",
                CommentText = "Test Comment",
                CommentTime = DateTime.Now
            };
            _commentRepository.AddComment(comment);

            // Act
            comment.Name = "Updated Commenter";
            _commentRepository.UpdateComment(comment);

            // Assert
            var updatedComment = _commentRepository.Find(comment.Id);
            Assert.NotNull(updatedComment);
            Assert.AreEqual(comment.Name, updatedComment.Name);
        }

        [Test]
        public void DeleteComment_ShouldDeleteCommentFromDatabase()
        {
            // Arrange
            var comment = new Comment
            {
                Name = "Test Commenter",
                Email = "test@example.com",
                CommentHeader = "Test Header",
                CommentText = "Test Comment",
                CommentTime = DateTime.Now
            };
            _commentRepository.AddComment(comment);

            // Act
            _commentRepository.DeleteComment(comment);

            // Assert
            var deletedComment = _commentRepository.Find(comment.Id);
            Assert.Null(deletedComment);
        }

        [Test]
        public void GetCommentsForPost_ShouldReturnCommentsForGivenPost()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test comment Post 1",
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
            _context.Posts.Add(post);
            _context.SaveChanges();

            var comment1 = new Comment
            {
                Name = "John Doe",
                Email = "john@example.com",
                Post = null, // Replace with an actual Post instance if needed
                CommentHeader = "Great post!",
                CommentText = "This is a fantastic article. I really enjoyed reading it."
                
            };

            var comment2 = new Comment
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Post = null, // Replace with an actual Post instance if needed
                CommentHeader = "Thank you!",
                CommentText = "Thank you for sharing this insightful post."
            };

            _commentRepository.AddComment(post.Id, comment1.Name, comment1.Email, comment1.Email, comment1.CommentText);
            _commentRepository.AddComment(post.Id, comment2.Name, comment2.Email, comment2.Email, comment2.CommentText);

            // Act
            var comments = _commentRepository.GetCommentsForPost(post);

            // Assert
            Assert.IsTrue(comments.Any(c => c.Name == comment1.Name));
            Assert.IsTrue(comments.Any(c => c.Name == comment2.Name));
        }

        [Test]
        public void GetCommentsForPostId_ShouldReturnCommentsForGivenPost()
        {
            // Arrange
            var post = new Post
            {
                Title = "Test comment Post 1",
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
            _context.Posts.Add(post);
            _context.SaveChanges();

            var comment1 = new Comment
            {
                Name = "John Doe",
                Email = "john@example.com",
                Post = null, // Replace with an actual Post instance if needed
                CommentHeader = "Great post!",
                CommentText = "This is a fantastic article. I really enjoyed reading it."

            };

            var comment2 = new Comment
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                Post = null, // Replace with an actual Post instance if needed
                CommentHeader = "Thank you!",
                CommentText = "Thank you for sharing this insightful post."
            };

            _commentRepository.AddComment(post.Id, comment1.Name, comment1.Email, comment1.Email, comment1.CommentText);
            _commentRepository.AddComment(post.Id, comment2.Name, comment2.Email, comment2.Email, comment2.CommentText);

            // Act
            var comments = _commentRepository.GetCommentsForPost(post.Id);

            // Assert
            Assert.IsTrue(comments.Any(c => c.Name == comment1.Name));
            Assert.IsTrue(comments.Any(c => c.Name == comment2.Name));
        }

        [Test]
        public void GetAllComments_ShouldReturnAllComments()
        {
            // Arrange
            var testData = new List<Comment>
{
    new Comment
    {
        Id = 1,
        Name = "John",
        Email = "john@example.com",
        PostId = 1,
        CommentHeader = "Header 1",
        CommentText = "This is the first comment.",
        CommentTime = DateTime.Now.AddDays(-5) // Adjust the date as needed
    },
    new Comment
    {
        Id = 2,
        Name = "Alice",
        Email = "alice@example.com",
        PostId = 1,
        CommentHeader = "Header 2",
        CommentText = "This is the second comment.",
        CommentTime = DateTime.Now.AddDays(-3) // Adjust the date as needed
    },
    new Comment
    {
        Id = 3,
        Name = "Bob",
        Email = "bob@example.com",
        PostId = 2,
        CommentHeader = "Header 1",
        CommentText = "This is another comment for a different post.",
        CommentTime = DateTime.Now.AddDays(-2) // Adjust the date as needed
    },
    // Add more comments as needed...
}.AsQueryable();

            Mock<DbSet<Comment>> mockDbSet = new Mock<DbSet<Comment>>();
            mockDbSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(testData.Provider);
            mockDbSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(testData.Expression);
            mockDbSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            mockDbSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(() => testData.GetEnumerator());
            _contextMock.Setup(c => c.Comments).Returns(mockDbSet.Object);

            // Act
            var result = _commentRepositoryMock.GetAllComments();

            // Assert
            Assert.AreEqual(testData.Count(), result.Count);
            foreach (var item in result)
            {
                Assert.True(testData.Contains(item));
            }
        }
    }

}
