using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class PaymentType
    {
        [Key]
        public int TypeID { get; set; }
        public string? TypeName { get; set; }
    }
}
