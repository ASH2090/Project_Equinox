using Microsoft.EntityFrameworkCore;
using Project_Equinox.Models.DomainModels;
using Project_Equinox.Models.Infrastructure;

namespace Project_Equinox.Models.Data.Repository
{
    public class EquinoxRepository : IEquinoxRepository
    {
        private readonly EquinoxContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public EquinoxRepository(EquinoxContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<Club> Clubs => GetRepository<Club>();
        public IRepository<ClassCategory> ClassCategories => GetRepository<ClassCategory>();
        public IRepository<User> Users => GetRepository<User>();
        public IRepository<EquinoxClass> EquinoxClasses => GetRepository<EquinoxClass>();
        public IRepository<Booking> Bookings => GetRepository<Booking>();

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_context);
            }
            return (IRepository<T>)_repositories[type];
        }

        public bool CanDeleteClub(int clubId)
        {
            // Check if the club has any classes associated with it
            var hasClasses = _context.Equinoxclasses
                .Any(c => c.ClubId == clubId);
            
            if (hasClasses)
                return false;

            // Check if any bookings exist for classes in this club
            var hasBookings = _context.Bookings
                .Include(b => b.EquinoxClass)
                .Any(b => b.EquinoxClass != null && b.EquinoxClass.ClubId == clubId);

            return !hasBookings;
        }

        public bool CanDeleteClassCategory(int categoryId)
        {
            // Check if the category has any classes associated with it
            var hasClasses = _context.Equinoxclasses
                .Any(c => c.ClassCategoryId == categoryId);
            
            if (hasClasses)
                return false;

            // Check if any bookings exist for classes in this category
            var hasBookings = _context.Bookings
                .Include(b => b.EquinoxClass)
                .Any(b => b.EquinoxClass != null && b.EquinoxClass.ClassCategoryId == categoryId);

            return !hasBookings;
        }

        public bool CanDeleteUser(int userId)
        {
            // Check if the user is a coach for any classes
            var isCoachForClasses = _context.Equinoxclasses
                .Any(c => c.CoachId == userId);
            
            if (isCoachForClasses)
                return false;

            // Check if any bookings exist for classes taught by this coach
            var hasBookingsForCoachClasses = _context.Bookings
                .Include(b => b.EquinoxClass)
                .Any(b => b.EquinoxClass != null && b.EquinoxClass.CoachId == userId);

            return !hasBookingsForCoachClasses;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
