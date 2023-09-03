using AutoMapper;
using FA.JustBlog.Core.Models;
using FA.JustBlog.Core.Repositories;
using JustBlog.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace JustBlog.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var allData = repository.GetAllCategories();
            var listModels = mapper.Map<List<CategoryModel>>(allData);
            return View(listModels);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = repository.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryModel model) {
            // validate model

            
            if (ModelState.IsValid)
            {
                var category = mapper.Map<Category>(model);
                category.UrlSlug = GenerateUrlSlug(model.Name);
                repository.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        [HttpPost]
        public IActionResult Delete(CategoryModel model)
        {
            var category = mapper.Map<Category>(model);
            repository.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = repository.Find(id);
            if (category == null)
            {
                
                return NotFound();
            }
            var model = mapper.Map<CategoryModel>(category);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(CategoryModel model)
        {
            // validate model
            if (ModelState.IsValid)
            {
                var category = mapper.Map<Category>(model);
                category.UrlSlug = GenerateUrlSlug(model.Name);
                // var category = repository.Find(model.Id);
                repository.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public string GenerateUrlSlug(string name)
        {
            string input = name.Trim();

            // Convert to lowercase and replace spaces with hyphens
            string slug = Regex.Replace(input, @"\s+", "-").ToLower();

            // Remove non-alphanumeric characters except hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Remove duplicate hyphens
            slug = Regex.Replace(slug, @"-{2,}", "-");

            return slug;
        }
    }
}
