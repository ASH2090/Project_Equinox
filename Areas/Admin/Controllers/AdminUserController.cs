using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_Equinox.Models;

namespace Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    public class AdminUserController : Controller
    {
        private readonly EquinoxContext _context;

        public AdminUserController(EquinoxContext context)
        {
            _context = context;
        }

        // GET: Admin/User
        [Route("")]
        [Route("Index")]
        public IActionResult Index() => View(_context.Users.ToList());
        [Route("Edit")]
        [Route("Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            User? model;
            if (id == null)
            {
                model = new User();
            }
            else
            {
                model = _context.Users.Find(id);
                if (model == null)
                {
                    return NotFound();
                }
            }
            return View(model);
        }
        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fix the errors.";
                return View(user);
            }

            if (user.UserId == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                _context.Users.Update(user);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [Route("Delete/{id?}")]
        public IActionResult Delete(int? id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AcceptVerbs("GET", "POST")]
        [Route("VerifyPhone")]
        public IActionResult VerifyPhone(string phoneNumber, int userId = 0)
        {
            var exists = _context.Users.Any(u => u.PhoneNumber == phoneNumber && u.UserId != userId);
            return Json(!exists);
        }
    }
}
