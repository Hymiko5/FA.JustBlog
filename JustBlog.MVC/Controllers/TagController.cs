using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using NuGet.Packaging;

namespace JustBlog.MVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository repository;

        public TagController(ITagRepository repository)
        {
            this.repository = repository;
        }

        // GET: Tag
        public async Task<IActionResult> Index(string? urlSlug)
        {
            IList<Tag> allTags = new List<Tag>();
            if(!string.IsNullOrEmpty(urlSlug))
            {
                var foundTag = repository.GetTagByUrlSlug(urlSlug);
                if(foundTag != null)
                {
                    allTags.Add(foundTag);
                }
               
            } else
            {
                allTags.AddRange(repository.GetAllTags());
            }
              return allTags != null ? 
                          View(allTags) :
                          Problem("Entity set 'JustBlogContext.Tags'  is null.");
        }

        // GET: Tag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UrlSlug,Description,Count")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                repository.AddTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tag = repository.Find(id);
            if (id == null || tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tag/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UrlSlug,Description,Count")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                repository.UpdateTag(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tag/Delete/5

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (repository.GetAllTags() == null)
            {
                return Problem("Entity set 'JustBlogContext.Tags'  is null.");
            }
            var tag = repository.Find(id);
            if (tag != null)
            {
                repository.DeleteTag(tag);
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
