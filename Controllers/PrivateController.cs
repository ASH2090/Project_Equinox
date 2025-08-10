using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class PrivateClassController : Controller
    {
        public IActionResult List(string id = "All")
        {
            return Content($"Area: Main, Controller: PrivateClass, Action: List, id: {id}");
        }
    }
}
