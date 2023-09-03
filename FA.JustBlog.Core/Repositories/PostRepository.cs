using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class PostRepository: GenericRepository<Post>, IPostRepository
    {
        private readonly JustBlogContext _context;
        public PostRepository(JustBlogContext context) :base(context)
        {
            _context = context;
        }

        public Post FindPost(int year, int? month, string? urlSlug)
        {
            return _context.Posts.FirstOrDefault(post =>
                post.PostedOn.Year == year &&
                post.PostedOn.Month == month &&
                post.UrlSlug == urlSlug);
        }

        public Post FindPost(int postId)
        {
            return _context.Posts.Find(postId);
        }

        public void AddPost(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));

            _context.Posts.Update(post);
            _context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }

        public void DeletePost(int postId)
        {
            var postToDelete = _context.Posts.Find(postId);
            if (postToDelete != null)
            {
                _context.Posts.Remove(postToDelete);
                _context.SaveChanges();
            }
        }

        public IList<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public IList<Post> GetPublishedPosts()
        {
            return _context.Posts.Where(post => post.Published).ToList();
        }
        public IList<Post> GetUnpublishedPosts()
        {
            return _context.Posts.Where(post => !post.Published).ToList();
        }

        public IList<Post> GetLatestPosts(int size)
        {
            return _context.Posts
                .Where(post => post.Published)
                .OrderByDescending(post => post.PostedOn)
                .Take(size)
                .ToList();
        }

        public IList<Post> GetPostsByMonth(DateTime monthYear)
        {
            var startDate = new DateTime(monthYear.Year, monthYear.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return _context.Posts
                .Where(post => post.Published && post.PostedOn >= startDate && post.PostedOn <= endDate)
                .ToList();
        }

        public int CountPostsForCategory(string category)
        {
            return _context.Posts
                .Count(post => post.Published && post.Category.Name.Contains(category));
        }

        public IList<Post> GetPostsByCategory(string category)
        {
            return _context.Posts
                .Where(post => post.Published && post.Category.Name.Contains(category))
                .ToList();
        }

        public int CountPostsForTag(string tag)
        {
            return _context.Posts
                .Count(post => post.Published && post.PostTagMaps.Any(pt => pt.Tag.Name.Contains(tag)));
        }

        public IList<Post> GetPostsByTag(string tag)
        {
            return _context.Posts.Where(p => p.PostTagMaps.Any(t => t.Tag.Name.Contains(tag))).ToList();
        }

        public IList<Post> GetMostViewedPost(int size)
        {
            return _context.Posts.OrderByDescending(p => p.ViewCount).Take(size).ToList();
        }

        public IList<Post> GetHighestPosts(int size)
        {
            return _context.Posts.OrderByDescending(p => p.Rate).Take(size).ToList();
        }
    }
}
