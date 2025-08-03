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
            model = _context.Clubs.Find(id)!;
            if (model == null) return NotFound();
        }
        return View(model);
    }

    [HttpPost]
    [Route("Edit")]
    public IActionResult Edit(Club club)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fix the error";
            return View(club);
        }

        if (club.ClubId == 0)
        {
            _context.Clubs.Add(club); // Create
        }
        else
        {
            _context.Clubs.Update(club); // Edit
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Route("Delete/{id}")]
    public IActionResult Delete(int id)
    {
        var club = _context.Clubs.Find(id);
        if (club == null) return NotFound();
        _context.Clubs.Remove(club);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [AcceptVerbs("Get", "Post")]
    [Route("VerifyClubPhone")]
    public IActionResult VerifyClubPhone(string phoneNumber, int? id)
    {
        var exists = _context.Clubs.Any(c => c.PhoneNumber == phoneNumber && c.ClubId != id);
        return Json(!exists);
    }
}
}
