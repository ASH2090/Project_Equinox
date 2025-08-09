using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.Util;
using System.Linq;
 
namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminClubController : Controller
    {
        private readonly IEquinoxRepository _repository;
 
        public AdminClubController(IEquinoxRepository repository)
        {
            _repository = repository;
        }
 
        [Route("Admin/Club")]
        [Route("Admin/Club/Index")]
        public IActionResult Index()
        {
            var options = new QueryOptions<Club>();
            var clubs = _repository.Clubs.List(options);
            return View(clubs);
        }
 
        [Route("Admin/Club/Edit")]
        [Route("Admin/Club/Edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            Club model;
            if (id == null)
            {
                model = new Club();
            }
            else
            {
                model = _repository.Clubs.Get(id.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }
            return View(model);
        }
 
        [HttpPost]
        [Route("Admin/Club/Edit")]
        [Route("Admin/Club/Edit/{id?}")]
        public IActionResult Edit(Club club)
        {
            // Server-side validation: Check for duplicate phone number
            var phoneOptions = new QueryOptions<Club>()
                .Filter(c => c.PhoneNumber == club.PhoneNumber && c.ClubId != club.ClubId);
            var existingClubs = _repository.Clubs.List(phoneOptions);
           
            if (existingClubs.Any())
            {
                ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
            }
 
            if (!ModelState.IsValid)
            {
                TempData["ValidationError"] = "Please correct the errors below and try again.";
                return View(club);
            }
 
            TempData.Remove("ValidationError");
 
            if (club.ClubId == 0)
            {
                _repository.Clubs.Add(club);
            }
            else
            {
                _repository.Clubs.Update(club);
            }
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
 
        [Route("Admin/Club/Delete/{id?}")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            // Business rule: Check if club can be deleted
            var canDelete = _repository.CanDeleteClub(id.Value);
            if (!canDelete)
            {
                TempData["ErrorMessage"] = "Cannot delete this club as it has classes with existing bookings.";
                return RedirectToAction(nameof(Index));
            }
 
            var club = _repository.Clubs.Get(id.Value);
            if (club == null)
            {
                return NotFound();
            }
 
            _repository.Clubs.Delete(club);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
 
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
 
 