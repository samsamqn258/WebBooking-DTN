using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class CancelBooking
    {
        [Key]
        public int CancelID { get; set; }
        public int BookingID { get; set; }
        public DateTime? CancelTime { get; set; }
        public string? Reason { get; set; }
        [ForeignKey("BookingID")]
        public Booking? Booking { get; set; }
    }
}
