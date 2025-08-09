using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project_Equinox.Models.DomainModels;

namespace Project_Equinox.Models.Data.Configuration
{
    public class ClassCategoryConfiguration : IEntityTypeConfiguration<ClassCategory>
    {
        public void Configure(EntityTypeBuilder<ClassCategory> builder)
        {
            builder.HasData(
                new ClassCategory { ClassCategoryId = 1, Name = "Boxing" },
                new ClassCategory { ClassCategoryId = 2, Name = "Yoga" },
                new ClassCategory { ClassCategoryId = 3, Name = "HIIT" },
                new ClassCategory { ClassCategoryId = 4, Name = "Strength" },
                new ClassCategory { ClassCategoryId = 5, Name = "Barre" },
                new ClassCategory { ClassCategoryId = 6, Name = "Sculpt" },
                new ClassCategory { ClassCategoryId = 7, Name = "Dancing" },
                new ClassCategory { ClassCategoryId = 8, Name = "Running" },
                new ClassCategory { ClassCategoryId = 9, Name = "Palate" }
            );
        }
    }
}
