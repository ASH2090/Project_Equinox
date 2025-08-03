using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.Validation;

namespace Project_Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        [Remote(action: "IsPhoneNumberAvailable", controller: "User", AdditionalFields = "UserId", ErrorMessage = "This phone number is already in use.")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in format: XXX-XXX-XXXX")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [AgeRange(8, 80)]
        public DateTime DOB { get; set; }

        [Display(Name = "Is Coach")]
        public bool IsCoach { get; set; }

        // Navigation property: One coach can teach many classes
        public ICollection<EquinoxClass>? Classes { get; set; }
    }
}
