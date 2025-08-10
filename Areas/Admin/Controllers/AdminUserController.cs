using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Infrastructure;
using Project_Equinox.Models.Util;
using System.Linq;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    public class AdminUserController : Controller
    {
        private readonly IEquinoxRepository _repository;

        public AdminUserController(IEquinoxRepository repository)
        {
            _repository = repository;
        }

        // GET: /Admin/User, /Admin/User/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var users = _repository.Users.List(new QueryOptions<User>()).ToList();
            return View(users);
        }


        // GET: /Admin/User/Edit or /Admin/User/Edit/5
        [HttpGet("Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            User? model = id == null ? new User() : _repository.Users.Get(id.Value);
            if (id != null && model == null) return NotFound();
            return View(model);
        }

        // POST: /Admin/User/Edit or /Admin/User/Edit/5
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            // uniqueness check for phone
            var options = new QueryOptions<User>
            {
                Where = u => u.PhoneNumber == user.PhoneNumber && u.UserId != user.UserId
            };
            if (_repository.Users.List(options).Any())
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(user);
            }

            TempData.Remove("ValidationError");

            if (user.UserId == 0) _repository.Users.Add(user);
            else _repository.Users.Update(user);

            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Prefer POST for destructive action; client does GET pre-check
        // POST: /Admin/User/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!_repository.CanDeleteUser(id))
            {
                TempData["ErrorMessage"] = "Cannot delete this coach because their classes have bookings.";
                return RedirectToAction(nameof(Index));
            }

            var user = _repository.Users.Get(id);
            if (user == null) return NotFound();

            _repository.Users.Delete(user);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Remote validation for phone uniqueness
        [AcceptVerbs("GET", "POST")]
        [Route("VerifyPhone")]
        public IActionResult VerifyPhone(string phoneNumber, int userId = 0)
        {
            var options = new QueryOptions<User>
            {
                Where = u => u.PhoneNumber == phoneNumber && u.UserId != userId
            };
            var exists = _repository.Users.List(options).Any();
            return Json(!exists);
        }
    }
}
