using System.ComponentModel.DataAnnotations;
namespace Project_Equinox.Models
{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string Name { get; set; } = string.Empty;
        // Navigation property: One category can have many classes
        public ICollection<EquinoxClass>? Classes { get; set; }
    }
}
