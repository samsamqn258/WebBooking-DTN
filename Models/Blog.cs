using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBooking.Models
{
    public class Blog
    {
         [Key]
        public int BlogID { get; set; }
        public string? BlogContent { get; set; }
        public string? PostTime { get; set; }
        public bool ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
    }
}
