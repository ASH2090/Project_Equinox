using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models;
using System.Linq;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Club")]
    public class AdminClubController : Controller
{
    private readonly EquinoxContext _context;

    public AdminClubController(EquinoxContext context)
    {
        _context = context;
    }

    [Route("")]
    [Route("Index")]
    public IActionResult Index() => View(_context.Clubs.ToList());

    [Route("Edit")]
    [Route("Edit/{id?}")]
    public IActionResult Edit(int? id)
    {
        Club model;
        if (id == null)
        {
            model = new Club();
        }
        else
        {
            var foundClub = _context.Clubs.Find(id);
            if (foundClub == null)
            {
                return NotFound();
            }
            model = foundClub;
        }
        return View(model);
    }

    [HttpPost]
    [Route("Edit")]
    [Route("Edit/{id?}")]
    public IActionResult Edit(Club club)
    {
        // Server-side validation: Check for duplicate phone number
        var phoneExists = _context.Clubs.Any(c => c.PhoneNumber == club.PhoneNumber && c.ClubId != club.ClubId);
        if (phoneExists)
        {
            ModelState.AddModelError("PhoneNumber", "This phone number is already registered.");
        }

        if (!ModelState.IsValid)
        {
            // Use TempData to coordinate with client-side validation
            TempData["ValidationError"] = "Please correct the errors below and try again.";
            return View(club);
        }

        // Clear any previous validation messages on successful validation
        TempData.Remove("ValidationError");

        if (club.ClubId == 0)
        {
            _context.Clubs.Add(club); // Create
        }
        else
        {
            _context.Clubs.Update(club); // Edit
        }
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var club = _context.Clubs.Find(id);
        if (club == null) return NotFound();
        _context.Clubs.Remove(club);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [AcceptVerbs("GET", "POST")]
    [Route("VerifyClubPhone")]
    public IActionResult VerifyClubPhone(string phoneNumber, int clubId = 0)
    {
        var exists = _context.Clubs.Any(c => c.PhoneNumber == phoneNumber && c.ClubId != clubId);
        return Json(!exists);
    }
}
}
