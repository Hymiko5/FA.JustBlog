using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class PostRepository: BaseService<Post>, IPostRepository
    {
        
        public PostRepository(IGenericRepository<Post> postRepository, IUnitOfWork unitOfWork) :base(unitOfWork, postRepository)
        {
            
        }

        public async Task<Post> FindPostAsync(int year, int? month, string? urlSlug)
        {
            return await unitOfWork.PostRepository.FirstOrDefaultAsync(post =>
                post.PostedOn.Year == year &&
                post.PostedOn.Month == month &&
                post.UrlSlug == urlSlug);
        }


        public async Task<IEnumerable<Post>> GetPublishedPostsAsync()
        {
            return await unitOfWork.PostRepository.FindAsync(post => post.Published);

        }
        public async Task<IEnumerable<Post>> GetUnpublishedPostsAsync()
        {
            return await unitOfWork.PostRepository.FindAsync(post => !post.Published);
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(int size)
        {
            return (await unitOfWork.PostRepository
                .FindAsync(post => post.Published))
                .OrderByDescending(post => post.PostedOn)
                .Take(size)
                .ToList();
        }

        public async Task<IEnumerable<Post>> GetPostsByMonthAsync(DateTime monthYear)
        {
            var startDate = new DateTime(monthYear.Year, monthYear.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await unitOfWork.PostRepository
                .FindAsync(post => post.Published && post.PostedOn >= startDate && post.PostedOn <= endDate);
        }

        public async Task<int> CountPostsForCategoryAsync(string category)
        {
            return await unitOfWork.PostRepository
                .CountAsync(post => post.Published && post.Category.Name.Contains(category));
        }

        public async Task<IEnumerable<Post>> GetPostsByCategoryAsync(string category)
        {
            return await unitOfWork.PostRepository
                .FindAsync(post => post.Published && post.Category.Name.Contains(category));
        }

        public async Task<int> CountPostsForTagAsync(string tag)
        {
            return await unitOfWork.PostRepository
                .CountAsync(post => post.Published && post.PostTagMaps.Any(pt => pt.Tag.Name.Contains(tag)));
        }

        public async Task<IEnumerable<Post>> GetPostsByTagAsync(string tag)
        {
            return await unitOfWork.PostRepository.FindAsync(p => p.PostTagMaps.Any(t => t.Tag.Name.Contains(tag)));
        }

        public async Task<IEnumerable<Post>> GetMostViewedPostAsync(int size)
        {
            return (await unitOfWork.PostRepository.GetAllAsync()).OrderByDescending(p => p.ViewCount).Take(size).ToList();
        }

        public async Task<IEnumerable<Post>> GetHighestPostsAsync(int size)
        {
            return (await unitOfWork.PostRepository.GetAllAsync()).OrderByDescending(p => p.Rate).Take(size).ToList();
        }
    }
}
