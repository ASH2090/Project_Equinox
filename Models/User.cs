using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Project_Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        public bool IsCoach { get; set; }
        // Navigation property: One coach can teach many classes
        public ICollection<EquinoxClass>? Classes { get; set; }
    }
}
