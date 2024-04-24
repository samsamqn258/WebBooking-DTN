using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }
        public string? HotelName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public double AverageScore {  get; set; } 
        public int TypeID { get; set; }
        public int AreaID { get; set; }
        public int UserID { get; set; }

        [ForeignKey("TypeID")]
        public HotelType? HotelType { get; set; }
        [ForeignKey("AreaID")]
        public Area? Area { get; set; }

        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
