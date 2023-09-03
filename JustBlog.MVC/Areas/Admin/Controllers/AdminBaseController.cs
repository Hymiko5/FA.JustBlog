using Microsoft.AspNetCore.Mvc;

namespace JustBlog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBaseController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
