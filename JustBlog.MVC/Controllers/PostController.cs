using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using JustBlog.MVC.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JustBlog.MVC.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository repository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITagRepository tagRepository;

        public PostController(IPostRepository repository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
            this.tagRepository = tagRepository;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
            SetViewData();
            
            var allData = repository.GetAllPosts();
            return View(allData);
        }
        [HttpPost]
        public IActionResult Index(PostQuery postQuery)
        {
            SetViewData();
            List<Post> allData = new List<Post>();
            if (postQuery != null)
            {
                if (postQuery.Published)
                {
                    allData.AddRange(repository.GetPublishedPosts());
                }
                else
                {
                    allData.AddRange(repository.GetUnpublishedPosts());
                }
                if (postQuery.LatestPostSize != null)
                {
                    allData.AddRange(repository.GetLatestPosts(postQuery.LatestPostSize));
                }
                if (postQuery.YearMonth != null)
                {
                    allData.AddRange(repository.GetPostsByMonth(postQuery.YearMonth));
                }
                if (postQuery.UrlSlug != null)
                {
                    var findByUrlPost = repository.FindPost(postQuery.YearMonth.Year, postQuery.YearMonth.Month, postQuery.UrlSlug);
                    if(findByUrlPost != null)
                    {
                        allData.Add(findByUrlPost);
                    }
                    
                }
                if (postQuery.Category != null)
                {
                    allData.AddRange(repository.GetPostsByCategory(postQuery.Category));
                }
                if (postQuery.Tag != null)
                {
                    allData.AddRange(repository.GetPostsByTag(postQuery.Tag));
                }
            }
            return View(allData);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategories(), "Id", "Id");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ShortDescription,PostContent,UrlSlug,Published,PostedOn,Modified,CategoryId,ViewCount,RateCount,TotalRate")] Post post)
        {
            if (ModelState.IsValid)
            {
                repository.AddPost(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategories(), "Id", "Name", post.CategoryId);
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || repository.GetAllPosts() == null)
            {
                return NotFound();
            }

            var post = repository.FindPost(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategories(), "Id", "Name", post!=null?post.CategoryId:0);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ShortDescription,PostContent,UrlSlug,Published,PostedOn,Modified,CategoryId,ViewCount,RateCount,TotalRate")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                    repository.UpdatePost(post);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetAllCategories(), "Id", "Id", post.CategoryId);
            return View(post);
        }

        // GET: Post/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Posts == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.Posts
        //        .Include(p => p.Category)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (repository.GetAllPosts() == null)
            {
                return Problem("Entity set 'JustBlogContext.Posts'  is null.");
            }
            var post = repository.FindPost(id);
            if (post != null)
            {
                repository.DeletePost(post);
            }
            
            return RedirectToAction(nameof(Index));
        }

        public void SetViewData() {
            var categories = categoryRepository.GetAllCategories();
            var categoryList = categories.Select(category => new SelectListItem
            {
                Text = category.Name, // Display text for the dropdown item
                Value = category.Name // Value of the dropdown item (usually an ID)
            }).ToList();
            ViewData["CategoryList"] = categoryList;
            var tags = tagRepository.GetAllTags();
            var tagList = tags.Select(tag => new SelectListItem
            {
                Text = tag.Name, // Display text for the dropdown item
                Value = tag.Name // Value of the dropdown item (usually an ID)
            }).ToList();
            ViewData["TagList"] = tagList;
        }

    }
}
