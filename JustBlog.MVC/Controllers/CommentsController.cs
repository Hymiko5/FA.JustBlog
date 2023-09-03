using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using JustBlog.MVC.Models;

namespace JustBlog.MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentRepository repository;
        private readonly IPostRepository postRepository;

        public CommentsController(ICommentRepository repository, IPostRepository postRepository)
        {
            this.repository = repository;
            this.postRepository = postRepository;
        }

        // GET: Comments
        public async Task<IActionResult> Index(int? postId)
        {
            var posts = postRepository.GetAllPosts();
            var postList = posts.Select(post => new SelectListItem
            {
                Text = post.Title, // Display text for the dropdown item
                Value = post.Id.ToString() // Value of the dropdown item (usually an ID)
            }).ToList();
            ViewData["PostList"] = postList;
            IEnumerable<Comment> allData;
            if(postId == null)
            {
                
                allData = repository.GetAllComments();
            } else
            {
                allData = repository.GetCommentsForPost(postId);
            }
            
            if (allData != null)
            {
                return View(allData);
            } else
            {
                return View();
            }
            
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var comment = repository.Find(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            var posts = postRepository.GetAllPosts();
            var postList = posts.Select(post => new SelectListItem
            {
                Text = post.Title, // Display text for the dropdown item
                Value = post.Id.ToString() // Value of the dropdown item (usually an ID)
            }).ToList();
            ViewData["PostList"] = postList;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentModel model)
        {
            if (ModelState.IsValid)
            {
                //var comment = new Comment() { PostId = model.PostId, Name = model.CommentName, Email = model.CommentEmail, CommentText = model.CommentBody, CommentHeader = model.CommentTitle, CommentTime = DateTime.Now };
                //repository.AddComment(comment);
                repository.AddComment(model.PostId, model.CommentName, model.CommentEmail, model.CommentTitle, model.CommentBody);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var posts = postRepository.GetAllPosts();
            var postList = posts.Select(post => new SelectListItem
            {
                Text = post.Title, // Display text for the dropdown item
                Value = post.Id.ToString() // Value of the dropdown item (usually an ID)
            }).ToList();
            ViewData["PostList"] = postList;
            var comment = repository.Find(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,CommentHeader,CommentText,CommentTime,PostId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // var category = repository.Find(model.Id);
                repository.UpdateComment(comment);
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }


        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = repository.Find(id);
            if (comment != null)
            {
                //repository.DeleteComment(id);
                repository.DeleteComment(comment);
            }
            
            return RedirectToAction(nameof(Index));
        }

    }
}
