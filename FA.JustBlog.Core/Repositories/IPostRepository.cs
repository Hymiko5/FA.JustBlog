using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public interface IPostRepository:IBaseService<Post>
    {
        Task<Post> FindPostAsync(int year, int? month, string? urlSlug);
        Task<IEnumerable<Post>> GetPublishedPostsAsync();
        Task<IEnumerable<Post>> GetUnpublishedPostsAsync();
        Task<IEnumerable<Post>> GetLatestPostsAsync(int size);
        Task<IEnumerable<Post>> GetPostsByMonthAsync(DateTime monthYear);
        Task<int> CountPostsForCategoryAsync(string category);
        Task<IEnumerable<Post>> GetPostsByCategoryAsync(string category);
        Task<int> CountPostsForTagAsync(string tag);
        Task<IEnumerable<Post>> GetPostsByTagAsync(string tag);

        Task<IEnumerable<Post>> GetMostViewedPostAsync(int size);
        Task<IEnumerable<Post>> GetHighestPostsAsync(int size);
    }
}
