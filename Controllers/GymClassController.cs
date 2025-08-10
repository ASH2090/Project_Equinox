
using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.Data.Repository;
using Project_Equinox.Models.ViewModels;
using Project_Equinox.Models.Infrastructure;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Util;

namespace Project_Equinox.Controllers
{

    public class GymClassController : Controller
    {
        private readonly IEquinoxRepository _repository;

        public GymClassController(IEquinoxRepository repository)
        {
            _repository = repository;
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
            vm.Clubs = _repository.Clubs.List(new QueryOptions<Club>()).ToList();
            vm.Categories = _repository.ClassCategories.List(new QueryOptions<ClassCategory>()).ToList();

            // Build query options for EquinoxClass
            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,Coach"
            };

            // Filtering
            if (vm.ClubId != 0 && vm.CategoryId != 0)
            {
                options.Where = c => c.ClubId == vm.ClubId && c.ClassCategoryId == vm.CategoryId;
            }
            else if (vm.ClubId != 0)
            {
                options.Where = c => c.ClubId == vm.ClubId;
            }
            else if (vm.CategoryId != 0)
            {
                options.Where = c => c.ClassCategoryId == vm.CategoryId;
            }

            vm.Classes = _repository.EquinoxClasses.List(options).ToList();
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
            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,Coach",
                Where = c => c.EquinoxClassId == id
            };
            var equinoxClass = _repository.EquinoxClasses.Get(options);
            if (equinoxClass == null)
            {
                return NotFound();
            }
            ViewBag.IsBooked = EquinoxCookie.IsClassBooked(Request, id);
            return View(equinoxClass);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(int id)
        {
            var bookingSessionId = EquinoxCookie.EnsureBookingSessionId(Request, Response);
            // Check if already booked in DB
            bool alreadyBooked = _repository.Bookings.List(new QueryOptions<Booking> {
                Where = b => b.EquinoxClassId == id && b.BookingSessionId == bookingSessionId
            }).Any();

            if (alreadyBooked)
            {
                TempData["Message"] = "Class is already booked.";
            }
            else
            {
                // Add to DB
                var booking = new Booking { EquinoxClassId = id, BookingSessionId = bookingSessionId };
                _repository.Bookings.Add(booking);
                _repository.Save();
                // Add to cookie for UI
                EquinoxCookie.SaveBooking(Response, id);
                TempData["Message"] = "Class booked successfully!";
            }
            return RedirectToAction("Index");
        }


        public IActionResult MyBookings()
        {
            var bookedIds = EquinoxCookie.GetBookedClasses(Request);
            var options = new QueryOptions<EquinoxClass>
            {
                Includes = "Club,ClassCategory,Coach",
                Where = c => bookedIds.Contains(c.EquinoxClassId)
            };
            var bookedClasses = _repository.EquinoxClasses.List(options).ToList();
            return View(bookedClasses);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelBooking(int id)
        {
            var bookingSessionId = EquinoxCookie.GetBookingSessionId(Request);
            if (string.IsNullOrEmpty(bookingSessionId))
            {
                TempData["Message"] = "Booking not found.";
                return RedirectToAction("MyBookings");
            }

            // Find booking in DB
            var booking = _repository.Bookings.List(new QueryOptions<Booking> {
                Where = b => b.EquinoxClassId == id && b.BookingSessionId == bookingSessionId
            }).FirstOrDefault();

            if (booking != null)
            {
                _repository.Bookings.Delete(booking);
                _repository.Save();
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