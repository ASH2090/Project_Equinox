using System.ComponentModel.DataAnnotations;

namespace Project_Equinox.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Enter the Name")]
        [StringLength(50, ErrorMessage = "Name should be not more than 50 characters.")]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
        public string Name { get; set; } = string.Empty;
        [Phone]
        [Required(ErrorMessage = "Enter a PhoneNumber.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
