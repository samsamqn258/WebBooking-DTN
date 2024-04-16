using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime? BookingTime { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public bool BookingStatus { get; set; }
        public bool PaymentStatus { get; set; }

        public double TotalPriceBooking { get; set; }
        public int RoomID { get; set; }
        public int UserID { get; set; }


        public Room? Room { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
