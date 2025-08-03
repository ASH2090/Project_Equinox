using Project_Equinox.Models;
using Microsoft.AspNetCore.Mvc;

namespace Project_Equinox.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class ManageClubController : Controller
    {
        private EquinoxContext context { get; set; }
        public ManageClubController(EquinoxContext ctx) => context = ctx;
        public IActionResult List()
        {
            var clubs = context.Club
                .OrderBy(m => m.Name)
                .ToList();

            return View(clubs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.action = "Add";
            return View("Edit", new Club());
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                if (club.ClubId == 0)
                {
                    context.Club.Add(club);
                    TempData["message"] = $"{club.Name} Added Successfully";
                }
                else
                {
                    context.Club.Update(club);
                    TempData["message"] = $"{club.Name} Updated Successfully";

                }
                context.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ViewBag.action = (club.ClubId == 0) ? "Add" : "Edit";
                return View("Edit", club);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.action = "Edit";
            var club = context.Club.Find(id);
            return View(club);
        }

        [HttpPost]
        public IActionResult Delete(Club club)
        {
            context.Club.Remove(club);
            TempData["message"] = $"{club.Name} Deleted Successfully";
            context.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Club club = context.Club
                    .FirstOrDefault(p => p.ClubId == id) ?? new Club();
            return View(club);
        }
    }
}
