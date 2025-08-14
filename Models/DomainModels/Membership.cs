using System.ComponentModel.DataAnnotations;

namespace Project_Equinox.Models.DomainModels
{
    public class Membership
    {
        public int MembershipId { get; set; }

        
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public double Price { get; set; }
    }
}
