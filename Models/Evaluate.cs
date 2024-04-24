using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Evaluate
    {
        [Key]
        public int EvaluateID { get; set; }
        public int PaymentID { get; set; }
        public string? Comment { get; set; }
        public int? Score { get; set; }
        public DateTime EvaluateTime {  get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        [ForeignKey("PaymentID")]

        public Payment? Payment { get; set; }


    }
}
