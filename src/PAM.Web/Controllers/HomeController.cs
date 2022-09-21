using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PAM.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace PAM.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
