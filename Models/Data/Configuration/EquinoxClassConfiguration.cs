using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Models.Data.Configuration
{
    public class EquinoxClassConfiguration : IEntityTypeConfiguration<EquinoxClass>
    {
        public void Configure(EntityTypeBuilder<EquinoxClass> builder)
        {
            builder.HasData(
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
        }
    }
}
