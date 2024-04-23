using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Payment
    {
        [Key]

        public int PaymentID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? TotalPayment { get; set; }
        public int TypeID { get; set; }
        public int BookingID { get; set; }
        [ForeignKey("TypeID")]
        public PaymentType? PaymentType { get; set; }
        [ForeignKey("BookingID")]

        public Booking? Booking { get; set; }
    }
}
