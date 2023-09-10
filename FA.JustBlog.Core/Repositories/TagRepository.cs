using FA.JustBlog.Core.BaseServices;
using FA.JustBlog.Core.Infrastructure;
using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class TagRepository: BaseService<Tag>,ITagRepository
    {
        public TagRepository(IGenericRepository<Tag> tagRepository, IUnitOfWork unitOfWork) :base(unitOfWork, tagRepository)
        {
        }

        public async Task<Tag> GetTagByUrlSlugAsync(string urlSlug)
        {
            return await unitOfWork.TagRepository.FirstOrDefaultAsync(tag => tag.UrlSlug == urlSlug);
        }
    }
}
