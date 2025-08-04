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
            User model;
            if (id == null)
            {
                model = new User();
            }
            else
            {
                var foundUser = _context.Users.Find(id);
                if (foundUser == null)
                {
                    return NotFound();
                }
                model = foundUser;
            }
            return View(model);
        }
        [HttpPost]
        [Route("Edit")]
        [Route("Edit/{id?}")]
        public IActionResult Edit(User user)
        {
            // Server-side validation: Check for duplicate phone number
            var phoneExists = _context.Users.Any(u => u.PhoneNumber == user.PhoneNumber && u.UserId != user.UserId);
            if (phoneExists)
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
            }

            if (!ModelState.IsValid)
            {
                // Use TempData to coordinate with client-side validation
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(user);
            }

            // Clear any previous validation messages on successful validation
            TempData.Remove("ValidationError");

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
