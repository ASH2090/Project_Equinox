using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models;

namespace Project_Equinox.Controllers
{
    public class GymClassController : Controller
    {
        private readonly EquinoxContext _context;

        public GymClassController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index(ClassFilterViewModel vm)
        {
            // Get session values if filters not set in request
            if (vm.ClubId == 0 && vm.CategoryId == 0)
            {
                var sessionVm = HttpContext.Session.GetFilterValues();
                vm.ClubId = sessionVm.ClubId;
                vm.CategoryId = sessionVm.CategoryId;
            }

            // Store current filter values in session
            HttpContext.Session.SaveFilterValues(vm);

            // Get all clubs and categories for dropdowns
            vm.Clubs = _context.Clubs.ToList();
            vm.Categories = _context.ClassCategories.ToList();

            // Build query with filters
            IQueryable<EquinoxClass> query = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach);

            if (vm.ClubId != 0)
                query = query.Where(c => c.ClubId == vm.ClubId);

            if (vm.CategoryId != 0)
                query = query.Where(c => c.ClassCategoryId == vm.CategoryId);

            // Get filtered classes
            vm.Classes = query.ToList();

            // Get booked class IDs for display purposes
            vm.BookedClassIds = EquinoxCookie.GetBookedClasses(Request);

            return View(vm);
        }

        public IActionResult Clear()
        {
            HttpContext.Session.ClearAllFilters();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var equinoxClass = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .FirstOrDefault(c => c.EquinoxClassId == id);

            if (equinoxClass == null)
            {
                return NotFound();
            }

            ViewBag.IsBooked = EquinoxCookie.IsClassBooked(Request, id);

            return View(equinoxClass);
        }

        public IActionResult Book(int id)
        {
            if (EquinoxCookie.IsClassBooked(Request, id))
            {
                TempData["Message"] = "Class is already booked.";
            }
            else
            {
                EquinoxCookie.SaveBooking(Response, id);
                TempData["Message"] = "Class booked successfully!";
            }

            return RedirectToAction("Index");
        }

        public IActionResult MyBookings()
        {
            var bookedIds = EquinoxCookie.GetBookedClasses(Request);

            var bookedClasses = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .Where(c => bookedIds.Contains(c.EquinoxClassId))
                .ToList();

            return View(bookedClasses);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            if (EquinoxCookie.IsClassBooked(Request, id))
            {
                EquinoxCookie.RemoveBooking(Response, id);
                TempData["Message"] = "Booking canceled successfully!";
            }
            else
            {
                TempData["Message"] = "Booking not found.";
            }

            return RedirectToAction("MyBookings");
        }
    }
}