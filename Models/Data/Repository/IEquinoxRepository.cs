using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Models.Data.Repository
{
    public interface IEquinoxRepository
    {
        // Generic repository access
        IRepository<Club> Clubs { get; }
        IRepository<ClassCategory> ClassCategories { get; }
        IRepository<User> Users { get; }
        IRepository<EquinoxClass> EquinoxClasses { get; }
        IRepository<Booking> Bookings { get; }
        
        // Business logic methods
        bool CanDeleteClub(int clubId);
        bool CanDeleteClassCategory(int categoryId);
        bool CanDeleteUser(int userId);
        
        // Save changes
        void Save();
    }
}
