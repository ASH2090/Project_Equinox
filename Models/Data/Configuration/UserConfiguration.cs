using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Models.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User { UserId = 1, Name = "Alex Smith", PhoneNumber = "312-111-2222", Email = "alex@equinox.com", DOB = new DateTime(1985, 5, 10), IsCoach = true },
                new User { UserId = 2, Name = "Maria Garcia", PhoneNumber = "312-111-3333", Email = "maria@equinox.com", DOB = new DateTime(1990, 3, 22), IsCoach = true },
                new User { UserId = 3, Name = "Priya Patel", PhoneNumber = "312-111-4444", Email = "priya@equinox.com", DOB = new DateTime(1992, 7, 15), IsCoach = true },
                new User { UserId = 4, Name = "John Lee", PhoneNumber = "312-111-5555", Email = "john@equinox.com", DOB = new DateTime(1988, 11, 2), IsCoach = true }
            );
        }
    }
}
