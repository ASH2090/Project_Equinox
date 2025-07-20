using Microsoft.AspNetCore.Mvc;

namespace Equinox.Controllers
{
    public class MembershipController : Controller
    {
        public IActionResult List(string id = "All")
        {
            return Content($"Area: Main, Controller: Membership, Action: List, id: {id}");
        }
    }
}
