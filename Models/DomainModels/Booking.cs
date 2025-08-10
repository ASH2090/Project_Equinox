namespace Project_Equinox.Models.DomainModels
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int EquinoxClassId { get; set; }
        public EquinoxClass? EquinoxClass { get; set; }
    public string BookingSessionId { get; set; } = string.Empty; // Used to associate bookings with a browser session
    }
}
