using FA.JustBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Repositories
{
    public class TagRepository: GenericRepository<Tag>,ITagRepository
    {
        private readonly JustBlogContext _context;
        public TagRepository(JustBlogContext context) :base(context)
        {
            _context = context;
        }

        public Tag Find(int TagId)
        {
            return _context.Tags.Find(TagId);
        }

        public void AddTag(Tag Tag)
        {
            if (Tag == null)
                throw new ArgumentNullException(nameof(Tag));

            _context.Tags.Add(Tag);
            _context.SaveChanges();
        }

        public void UpdateTag(Tag Tag)
        {
            if (Tag == null)
                throw new ArgumentNullException(nameof(Tag));

            _context.Tags.Update(Tag);
            _context.SaveChanges();
        }

        public void DeleteTag(Tag Tag)
        {
            if (Tag != null)
            {
                _context.Tags.Remove(Tag);
                _context.SaveChanges();
            }
        }

        public void DeleteTag(int TagId)
        {
            var tagToDelete = _context.Tags.Find(TagId);
            if (tagToDelete != null)
            {
                _context.Tags.Remove(tagToDelete);
                _context.SaveChanges();
            }
        }

        public IList<Tag> GetAllTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetTagByUrlSlug(string urlSlug)
        {
            return _context.Tags.FirstOrDefault(tag => tag.UrlSlug == urlSlug);
        }
    }
}
