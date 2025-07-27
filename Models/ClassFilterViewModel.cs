using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project_Equinox.Models
{
    public class ClassFilterViewModel
    {
        public int ClubId { get; set; }
        public int CategoryId { get; set; }

        public List<Club> Clubs { get; set; } = new List<Club>();
        public List<ClassCategory> Categories { get; set; } = new List<ClassCategory>();
        public List<EquinoxClass> Classes { get; set; } = new List<EquinoxClass>();
        public List<int> BookedClassIds { get; set; } = new List<int>();
        public Boolean IsClassBooked(int classId)
        {
            return BookedClassIds?.Contains(classId) ?? false;
        }
        public class ClassDetailViewModel
        {
            public EquinoxClass Classes { get; set; } = new EquinoxClass();
            public Boolean IsBooked { get; set; }
            public int ClubId { get; set; }
            public int CategoryId { get; set; }
            public string returnUrl { get; set; } = string.Empty;
            
        }

    }

}
