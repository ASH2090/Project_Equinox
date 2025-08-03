using Microsoft.EntityFrameworkCore;

namespace Project_Equinox.Models
{
    public class EquinoxContext : DbContext
    {
        public EquinoxContext(DbContextOptions<EquinoxContext> options)
            : base(options) { }

        public DbSet<Club> Club { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasData(
                new Club { ClubId = 1, Name = "Bronzeville", PhoneNumber = "778-404-0404" },
                new Club { ClubId = 2, Name = "Andersonville", PhoneNumber = "892-505-0505" },
                new Club { ClubId = 3, Name = "Rogers Park", PhoneNumber = "851-606-0606" }
            );
        }
    }
}
