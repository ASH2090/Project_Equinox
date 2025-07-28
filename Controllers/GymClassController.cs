using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models;

namespace Project_Equinox.Controllers
{
    public class GymClassController : Controller
    {
        private readonly EquinoxContext _context;

        // Constants to avoid duplicate message strings
        private const string BookingSuccessMessage = "Class booked successfully!";
        private const string BookingExistsMessage = "Class is already booked.";
        private const string CancelSuccessMessage = "Booking canceled successfully!";
        private const string BookingNotFoundMessage = "Booking not found.";

        public GymClassController(EquinoxContext context)
        {
            _context = context;
        }

        public IActionResult Index(ClassFilterViewModel vm)
        {
            try
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
            catch (Exception ex)
            {
                return Content(
                    $"<h2>🔥 Error in GymClassController.Index()</h2>" +
                    $"<strong>Message:</strong> {ex.Message}<br/><br/>" +
                    $"<strong>StackTrace:</strong><pre>{ex.StackTrace}</pre>" +
                    $"<br/><strong>Inner Exception:</strong> {ex.InnerException?.Message ?? "None"}",
                    "text/html"
                );
            }
        }

        public IActionResult Clear()
        {
            HttpContext.Session.ClearFilterValues();
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var query = _context.Equinoxclasses
                .Include(c => c.Club)
                .Include(c => c.ClassCategory);

            var equinoxClass = query.FirstOrDefault(c => c.EquinoxClassId == id);

            if (equinoxClass == null)
            {
                return NotFound();
            }

            var bookedIds = CookieHelper.GetBookedClassIds(Request);
            ViewBag.IsBooked = bookedIds.Contains(id);

            return View(equinoxClass);
        }

        public IActionResult Book(int id)
        {
            var bookedIds = CookieHelper.GetBookedClassIds(Request);

            if (!bookedIds.Contains(id))
            {
                bookedIds.Add(id);
                TempData["Message"] = BookingSuccessMessage;
            }
            else
            {
                TempData["Message"] = BookingExistsMessage;
            }

            CookieHelper.SetBookedClassIds(Response, bookedIds);
            return RedirectToAction("Index");
        }

        public IActionResult MyBookings()
        {
            var bookedIds = CookieHelper.GetBookedClassIds(Request);

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

        // Temporary endpoint to initialize database on Azure
        public IActionResult InitDatabase()
        {
            try
            {
                _context.Database.EnsureCreated();
                return Content("Database initialized successfully! You can now use the application.");
            }
            catch (Exception ex)
            {
                return Content($"Database initialization failed: {ex.Message}");
            }
        }
    }
}