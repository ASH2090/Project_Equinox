
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Project_Equinox.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        [Required(ErrorMessage = "Club name is required.")]
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        // Navigation property: One club can have many classes
        public ICollection<EquinoxClass>? Classes { get; set; }
    }
}
