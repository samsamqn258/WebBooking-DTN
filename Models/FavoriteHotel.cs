using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class FavoriteHotel
    {
        [Key]

        public int HotelID { get; set; }
        [Key]

        public int UserID { get; set; }
        [ForeignKey("HotelID")]
        public Hotel? Hotel { get; set; }
        [ForeignKey("UserID")]

        public User? User { get; set; }
    }
}
