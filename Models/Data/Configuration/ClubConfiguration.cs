using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Models.Data.Configuration
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasData(
                new Club { ClubId = 1, Name = "Chicago Loop", PhoneNumber = "331-567-7657" },
                new Club { ClubId = 2, Name = "West Chicago", PhoneNumber = "331-678-3456" },
                new Club { ClubId = 3, Name = "Lincoln Park", PhoneNumber = "431-658-3256" }
            );
        }
    }
}
