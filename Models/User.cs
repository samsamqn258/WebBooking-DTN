using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class User
    {
        [Key]

        public int UserID { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public int? UserType { get; set; }
        public string? Image { get; set; }
    }
}
