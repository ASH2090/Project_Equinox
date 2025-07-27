using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project_Equinox.Models
{
    public class ClassFilterViewModel
    {
        public int ClubId { get; set; }
        public int CategoryId { get; set; }

        public List<Club> Clubs { get; set; } = new List<Club>();
        public List<ClassCategory> Categories { get; set; } = new List<ClassCategory>();
        public List<EquinoxClass> EquinoxClasses { get; set; } = new List<EquinoxClass>();
        public List<int> BookClassIds { get; set; } = new List<int>();
        public Boolean IsClassBooked(int classId)
        {
            return BookClassIds?.Contains(classId) ?? false;
        }
        public class ClassDetailViewModel
        {
            public EquinoxClass EquinoxClass { get; set; } = new EquinoxClass();
            public Boolean IsBooked { get; set; }
            public int ClubId { get; set; }
            public int CategoryId { get; set; }
            public string returnUrl { get; set; } = string.Empty;
            
        }

    }

}
