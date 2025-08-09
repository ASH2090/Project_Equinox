using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Project_Equinox.Models.Util;

namespace Project_Equinox.Models.DomainModels
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Alphanumeric only.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in format: XXX-XXX-XXXX")]
        [Remote("VerifyPhone", "AdminUser", AdditionalFields = "UserId", ErrorMessage = "This phone number is already registered.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/1/1944", "1/1/2017", ErrorMessage = "Age must be between 8 and 80 years.")]
        [MinimumAge(8, 80)]
        public DateTime DOB { get; set; }

        [Display(Name = "Is Coach")]
        public bool IsCoach { get; set; }

        public ICollection<EquinoxClass>? Classes { get; set; }
    }
}
