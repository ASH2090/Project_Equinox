using System.ComponentModel.DataAnnotations;

public class ClassCategory
{
    public int ClassCategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Alphanumeric only.")]
    public string Name { get; set; } = string.Empty;

    public string Image { get; set; } = string.Empty;
}
