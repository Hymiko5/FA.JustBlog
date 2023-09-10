using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        private IGenericRepository<Comment> _genericRepository;
        private UnitOfWork _unitOfWork;
        private CommentRepository _commentRepository;
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
            _genericRepository = new GenericRepository<Comment>(_context);
            _unitOfWork = new UnitOfWork(_context);
            _commentRepository = new CommentRepository(_unitOfWork, _genericRepository);
        }

        [Test]
        public async Task AddComment_ShouldAddCommentToDatabase()
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
            await _commentRepository.AddAsync(comment);

            // Assert
            var addedComment = await _commentRepository.GetByIdAsync(comment.Id);
            Assert.NotNull(addedComment);
            Assert.AreEqual(comment.Name, addedComment.Name);
        }

        [Test]
        public async Task AddCommentWithPostId_ShouldAddCommentWithAssociatedPost()
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
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            await _unitOfWork.PostRepository.AddAsync(post);
       
            // Act
            await _commentRepository.AddCommentAsync(post.Id, "Test Commenter", "test@example.com", "Test Header", "Test Comment");
            var p = _unitOfWork.PostRepository.GetByIdAsync(post.Id);
            Console.WriteLine(p);
            // Assert
            var addedComment = (await _commentRepository.GetCommentsForPostAsync(post.Id)).FirstOrDefault();
            Assert.NotNull(addedComment);
            Assert.AreEqual(post.Id, addedComment.Post.Id);
        }

        [Test]
        public async Task UpdateComment_ShouldUpdateCommentInDatabase()
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
            await _commentRepository.AddAsync(comment);

            // Act
            comment.Name = "Updated Commenter";
            await _commentRepository.UpdateAsync(comment);

            // Assert
            var updatedComment = await _commentRepository.GetByIdAsync(comment.Id);
            Assert.NotNull(updatedComment);
            Assert.AreEqual(comment.Name, updatedComment.Name);
        }

        [Test]
        public async Task DeleteComment_ShouldDeleteCommentFromDatabase()
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
            await _commentRepository.AddAsync(comment);
            // Act
            await _commentRepository.DeleteAsync(comment);

            // Assert
            var deletedComment = await _commentRepository.GetByIdAsync(comment.Id);
            Assert.Null(deletedComment);
        }

        [Test]
        public async Task GetCommentsForPost_ShouldReturnCommentsForGivenPost()
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
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            await _unitOfWork.PostRepository.AddAsync(post);

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
            
            await _commentRepository.AddCommentAsync(post.Id, comment1.Name, comment1.Email, comment1.Email, comment1.CommentText);
            await _commentRepository.AddCommentAsync(post.Id, comment2.Name, comment2.Email, comment2.Email, comment2.CommentText);

            // Act
            var comments = await _commentRepository.GetCommentsForPostAsync(post);

            // Assert
            Assert.IsTrue(comments.Any(c => c.Name == comment1.Name));
            Assert.IsTrue(comments.Any(c => c.Name == comment2.Name));
        }

        [Test]
        public async Task GetCommentsForPostId_ShouldReturnCommentsForGivenPost()
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
                PostTagMaps = new List<PostTagMap>(), // Create a list of PostTagMap if needed
                ViewCount = 0,
                RateCount = 0,
                TotalRate = 0
            };
            await _unitOfWork.PostRepository.AddAsync(post);

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

            await _commentRepository.AddCommentAsync(post.Id, comment1.Name, comment1.Email, comment1.Email, comment1.CommentText);
            await _commentRepository.AddCommentAsync(post.Id, comment2.Name, comment2.Email, comment2.Email, comment2.CommentText);

            // Act
            var comments = await _commentRepository.GetCommentsForPostAsync(post.Id);

            // Assert
            Assert.IsTrue(comments.Any(c => c.Name == comment1.Name));
            Assert.IsTrue(comments.Any(c => c.Name == comment2.Name));
        }

        [Test]
        public async Task GetAllComments_ShouldReturnAllComments()
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
                };
            await _commentRepository.AddAsync(testData[0]);
            await _commentRepository.AddAsync(testData[1]);
            await _commentRepository.AddAsync(testData[2]);


            // Act
            var result = await _commentRepository.GetAllAsync();

            // Assert
            Assert.AreEqual(testData.Count(), result.Count());
            foreach (var item in result)
            {
                Assert.True(testData.Contains(item));
            }
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _context.Database.EnsureDeleted();
        }
    }

}
