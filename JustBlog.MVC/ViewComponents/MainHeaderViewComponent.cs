using FA.JustBlog.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JustBlog.MVC.ViewComponents
{
    [ViewComponent]
    public class MainHeaderViewComponent: ViewComponent
    {
        private readonly ICategoryRepository repository;

        public MainHeaderViewComponent(ICategoryRepository repository)
        {
            this.repository = repository;
        }

        [ResponseCache]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Perform any necessary logic or data retrieval here

            // Example: Fetch some data from a service
            var data = repository.GetAllCategories();

            // Pass the data to the view
            return View(data);
        }

    }
}
