using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Hotel
    {
        public int HotelID { get; set; }
        public string? HotelName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public int TypeID { get; set; }
        public int AreaID { get; set; }
        public int UserID { get; set; }

        public HotelType? HotelType { get; set; }
        public Area? Area { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
