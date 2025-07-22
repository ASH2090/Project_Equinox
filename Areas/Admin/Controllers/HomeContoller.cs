using Microsoft.AspNetCore.Mvc;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        //The requirement for routing in Admin area is to use Attribute Routing
        //So we need to decorate the action method with [Route] attribute
        public IActionResult Index() => View();
    }
}
