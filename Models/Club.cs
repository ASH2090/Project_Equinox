using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

public class Club
{
    public int ClubId { get; set; }

    [Required(ErrorMessage = "Club name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Alphanumeric only.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number.")]
    [RegularExpression(@"^(\+?1[-.\s]?)?(\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4})$", ErrorMessage = "Phone number must be in format: (123) 456-7890 or 123-456-7890")]
    [Remote("VerifyClubPhone", "AdminClub", "Admin", ErrorMessage = "Phone number already exists.")]
    public string PhoneNumber { get; set; } = string.Empty;
}
