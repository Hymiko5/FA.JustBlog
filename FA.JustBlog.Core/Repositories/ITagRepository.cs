using FA.JustBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public interface ITagRepository
    {
        Tag Find(int TagId);
        void AddTag(Tag Tag);
        void UpdateTag(Tag Tag);
        void DeleteTag(Tag Tag);
        void DeleteTag(int TagId);
        IList<Tag> GetAllTags();
        Tag GetTagByUrlSlug(string urlSlug);
    }
}
