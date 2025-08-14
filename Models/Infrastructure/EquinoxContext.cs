    
using Microsoft.EntityFrameworkCore;

using Project_Equinox.Models.DomainModels;
namespace Project_Equinox.Models.Infrastructure
{
    public class EquinoxContext : DbContext
    {
        public EquinoxContext(DbContextOptions<EquinoxContext> options)
            : base(options)
        { }
        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<EquinoxClass> Equinoxclasses { get; set; } = null!;
        public DbSet<ClassCategory> ClassCategories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Membership> Memberships { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>()
                .HasData(
                    new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "331-567-7657" },
                    new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "331-678-3456" },
                    new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "431-658-3256" }
                );

            modelBuilder.Entity<ClassCategory>()
                .HasData(
                    new ClassCategory { ClassCategoryId = 1, Name = "Boxing", },
                    new ClassCategory { ClassCategoryId = 2, Name = "Yoga", },
                    new ClassCategory { ClassCategoryId = 3, Name = "HIIT", },
                    new ClassCategory { ClassCategoryId = 4, Name = "Strength", },
                    new ClassCategory { ClassCategoryId = 5, Name = "Barre", },
                    new ClassCategory { ClassCategoryId = 6, Name = "Sculpt", },
                    new ClassCategory { ClassCategoryId = 7, Name = "Dancing", },
                    new ClassCategory { ClassCategoryId = 8, Name = "Running", },
                    new ClassCategory { ClassCategoryId = 9, Name = "Palate", }
                );

            modelBuilder.Entity<User>()
                .HasData(
                    new User { UserId = 1, Name = "Alex Smith", PhoneNumber = "312-111-2222", Email = "alex@equinox.com", DOB = new DateTime(1985, 5, 10), IsCoach = true },
                new User { UserId = 2, Name = "Maria Garcia", PhoneNumber = "312-111-3333", Email = "maria@equinox.com", DOB = new DateTime(1990, 3, 22), IsCoach = true },
                new User { UserId = 3, Name = "Priya Patel", PhoneNumber = "312-111-4444", Email = "priya@equinox.com", DOB = new DateTime(1992, 7, 15), IsCoach = true },
                new User { UserId = 4, Name = "John Lee", PhoneNumber = "312-111-5555", Email = "john@equinox.com", DOB = new DateTime(1988, 11, 2), IsCoach = true }
                );
            modelBuilder.Entity<EquinoxClass>()
                .HasData(
                new EquinoxClass
                {
                    EquinoxClassId = 1,
                    Name = "Boxing 101",
                    ClassPicture = "/images/boxing101.jpg",
                    ClassDay = "Monday",
                    Time = "8 AM – 9 AM",
                    ClassCategoryId = 1,
                    ClubId = 1,
                    CoachId = 1
                },
                new EquinoxClass
                {
                    EquinoxClassId = 2,
                    Name = "Yoga Flow",
                    ClassPicture = "/images/yogaflow.jpg",
                    ClassDay = "Tuesday",
                    Time = "6 PM – 7 PM",
                    ClassCategoryId = 2,
                    ClubId = 2,
                    CoachId = 2
                },
                new EquinoxClass
                {
                    EquinoxClassId = 3,
                    Name = "HIIT Blast",
                    ClassPicture = "/images/hiitblast.jpg",
                    ClassDay = "Wednesday",
                    Time = "5 PM – 6 PM",
                    ClassCategoryId = 3,
                    ClubId = 3,
                    CoachId = 3
                },
                new EquinoxClass
                {
                    EquinoxClassId = 4,
                    Name = "Strength Training",
                    ClassPicture = "/images/strengthtraining.jpg",
                    ClassDay = "Thursday",
                    Time = "7 AM – 8 AM",
                    ClassCategoryId = 4,
                    ClubId = 1,
                    CoachId = 4
                },
                new EquinoxClass
                {
                    EquinoxClassId = 5,
                    Name = "Barre Basics",
                    ClassPicture = "/images/barrebasic.jpg",
                    ClassDay = "Friday",
                    Time = "9 AM – 10 AM",
                    ClassCategoryId = 5,
                    ClubId = 2,
                    CoachId = 1
                }
                );

            modelBuilder.Entity<Membership>()
                .HasData(
                    new Membership { MembershipId = 1, Name = "Annual", Price = 499.99 },
                    new Membership { MembershipId = 2, Name = "Monthly", Price = 49.99 },
                    new Membership { MembershipId = 3, Name = "Punch Card", Price = 99.99 }
                );

        }
    }
}
