using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }
        public int LockID { get; set; }
    }
}
