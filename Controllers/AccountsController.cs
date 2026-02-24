using Microsoft.AspNetCore.Mvc;

namespace BasicAuth.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
