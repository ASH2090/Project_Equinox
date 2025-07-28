using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models;

namespace Project_Equinox.Controllers
{
    public class ClassesController : Controller
    {
        private readonly EquinoxContext _context;
        
        // Constants to avoid duplicate message strings
        private const string BookingSuccessMessage = "Class booked successfully!";
        private const string BookingExistsMessage = "Class is already booked.";
        private const string CancelSuccessMessage = "Booking canceled successfully!";
        private const string BookingNotFoundMessage = "Booking not found.";

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

        public IActionResult Clear()
        {
            // Clear session filter values
            HttpContext.Session.ClearFilterValues();
            
            // Redirect to index with no parameters to show all classes
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            // Reuse same Include pattern to avoid duplication
            var query = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory);
                
            var equinoxClass = query.FirstOrDefault(c => c.EquinoxClassId == id);

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
                TempData["Message"] = BookingSuccessMessage;
            }
            else
            {
                TempData["Message"] = BookingExistsMessage;
            }

            // Save updated list back to cookie
            CookieHelper.SetBookedClassIds(Response, bookedIds);

            // PRG Pattern: Redirect to Filter page as per requirements
            return RedirectToAction("Index");
        }

        public IActionResult MyBookings()
        {
            var bookedIds = CookieHelper.GetBookedClassIds(Request);
            
            // Reuse same Include pattern as Index method to avoid duplication
            var query = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory);
                
            var bookedClasses = query
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
                TempData["Message"] = CancelSuccessMessage;
            }
            else
            {
                TempData["Message"] = BookingNotFoundMessage;
            }
            
            return RedirectToAction("MyBookings");
        }
    }
}