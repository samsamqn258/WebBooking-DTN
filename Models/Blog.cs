using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class Blog
    {
        
        public int BlogID { get; set; }
        public string? BlogContent { get; set; }
        public string? PostTime { get; set; }
        public bool ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }

        public User? User { get; set; }
    }
}
