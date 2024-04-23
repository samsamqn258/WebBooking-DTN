using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Room
    {
        [Key]

        public int RoomID { get; set; }
        public string? RoomNumber { get; set; }
 
        public double? PricePerNight { get; set; }
        public string? Amenities { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public bool IsActive { get; set; }
        public double Discount { get; set; }
        public int HotelID { get; set; }
        public int TypeID { get; set; }
        [ForeignKey("HotelID")]

        public Hotel? Hotel { get; set; }
        [ForeignKey("TypeID")]

        public RoomType? RoomType { get; set; }

    }
}
