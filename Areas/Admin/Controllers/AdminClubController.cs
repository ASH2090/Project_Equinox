using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.Util;
using System.Linq;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Club")]
    public class AdminClubController : Controller
    {
        private readonly IEquinoxRepository _repository;

        public AdminClubController(IEquinoxRepository repository)
        {
            _repository = repository;
        }

        // GET /Admin/Club  and /Admin/Club/Index
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var clubs = _repository.Clubs.List(new QueryOptions<Club>());
            return View(clubs);
        }


        // GET /Admin/Club/Edit or /Admin/Club/Edit/5
        [HttpGet("Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            Club? model = id == null ? new Club() : _repository.Clubs.Get(id.Value);
            if (id != null && model == null) return NotFound();
            return View(model);
        }

        // POST /Admin/Club/Edit or /Admin/Club/Edit/5
        [HttpPost("Edit/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Club club)
        {
            // Server-side uniqueness check for phone
            var phoneOptions = new QueryOptions<Club>()
                .Filter(c => c.PhoneNumber == club.PhoneNumber && c.ClubId != club.ClubId);

            if (_repository.Clubs.List(phoneOptions).Any())
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
            }

            if (!ModelState.IsValid)
            {
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(club);
            }

            TempData.Remove("ValidationError");

            if (club.ClubId == 0) _repository.Clubs.Add(club);
            else _repository.Clubs.Update(club);

            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        // (Recommended) POST delete; if you must keep GET, remove ValidateAntiForgeryToken
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Business rule: guard delete
            if (!_repository.CanDeleteClub(id))
            {
                TempData["ErrorMessage"] = "Cannot delete this club as it has classes with existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            var club = _repository.Clubs.Get(id);
            if (club == null) return NotFound();

            _repository.Clubs.Delete(club);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }

        // Remote validation for phone uniqueness
        [AcceptVerbs("GET", "POST")]
        [Route("VerifyClubPhone")]
        public IActionResult VerifyClubPhone(string phoneNumber, int clubId = 0)
        {
            var options = new QueryOptions<Club>()
                .Filter(c => c.PhoneNumber == phoneNumber && c.ClubId != clubId);
            var existingClubs = _repository.Clubs.List(options);
            return Json(!existingClubs.Any());
        }
    }
}
