using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models;

namespace Project_Equinox.Controllers
{
    public class ClassesController : Controller
    {
        private readonly EquinoxContext _context;

        public ClassesController(EquinoxContext context)
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
            HttpContext.Session.SetFilterValues(vm);

            // Get all clubs and categories for dropdowns FIRST
            vm.Clubs = _context.Clubs.ToList();
            vm.Categories = _context.ClassCategories.ToList();

            // Build query with filters
            var query = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Include(c => c.Coach)
                .AsQueryable();

            if (vm.ClubId != 0)
                query = query.Where(c => c.ClubId == vm.ClubId);

            if (vm.CategoryId != 0)
                query = query.Where(c => c.ClassCategoryId == vm.CategoryId);

            // Get filtered classes
            vm.Classes = query.ToList();

            // Get booked class IDs for display purposes
            vm.BookedClassIds = CookieHelper.GetBookedClassIds(Request);

            return View(vm);
        }

        public IActionResult Detail(int id)
        {
            var equinoxClass = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .FirstOrDefault(c => c.EquinoxClassId == id);

            if (equinoxClass == null)
            {
                return NotFound();
            }

            // Check if class is already booked
            var bookedIds = CookieHelper.GetBookedClassIds(Request);
            ViewBag.IsBooked = bookedIds.Contains(id);
            
            return View(equinoxClass);
        }

        public IActionResult Book(int id)
        {
            // Get current booked classes from cookie
            var bookedIds = CookieHelper.GetBookedClassIds(Request);

            // Add the new class if not already booked
            if (!bookedIds.Contains(id))
            {
                bookedIds.Add(id);
                TempData["Message"] = "Class booked successfully!";
            }
            else
            {
                TempData["Message"] = "Class is already booked.";
            }

            // Save updated list back to cookie
            CookieHelper.SetBookedClassIds(Response, bookedIds);

            return RedirectToAction("MyBookings");
        }

        public IActionResult MyBookings()
        {
            var bookedIds = CookieHelper.GetBookedClassIds(Request);
            var bookedClasses = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory)
                .Where(c => bookedIds.Contains(c.EquinoxClassId))
                .ToList();
            
            return View(bookedClasses);
        }

        [HttpPost]
        public IActionResult CancelBooking(int id)
        {
            var bookedIds = CookieHelper.GetBookedClassIds(Request);
            if (bookedIds.Contains(id))
            {
                bookedIds.Remove(id);
                CookieHelper.SetBookedClassIds(Response, bookedIds);
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