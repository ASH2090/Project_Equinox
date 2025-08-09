using System.ComponentModel.DataAnnotations;

namespace Project_Equinox.Models.DomainModels
{
    public class EquinoxClass
    {
        public int EquinoxClassId { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        public string Name { get; set; } = string.Empty;

        public string ClassPicture { get; set; } = string.Empty;
        public string ClassDay { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;

        public int ClassCategoryId { get; set; }
        public ClassCategory? ClassCategory { get; set; }

        public int ClubId { get; set; }
        public Club? Club { get; set; }

        public int CoachId { get; set; }
        public User? Coach { get; set; }
    }
}
