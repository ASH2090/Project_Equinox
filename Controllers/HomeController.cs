 using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Contact() =>
            Content("Area: Main, Controller: Home, Action: Contact");

        public IActionResult Privacy() => View();

        public IActionResult Terms() =>
            Content("Area: Main, Controller: Home, Action: Terms");

        public IActionResult CookiePolicy() =>
            Content("Area: Main, Controller: Home, Action: CookiePolicy");
    }
}