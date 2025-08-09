using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace Project_Equinox.Models.DomainModels
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Club name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Alphanumeric only.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in format: XXX-XXX-XXXX")]
        [Remote("VerifyClubPhone", "AdminClub", AdditionalFields = "ClubId", ErrorMessage = "This phone number is already registered.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
