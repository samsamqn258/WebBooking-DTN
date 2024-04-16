namespace WebBooking.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? TotalPayment { get; set; }
        public int TypeID { get; set; }
        public int BookingID { get; set; }

        public PaymentType? PaymentType { get; set; }
        public Booking? Booking { get; set; }
    }
}
