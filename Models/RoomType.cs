using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class RoomType
    {
        [Key]
        public int TypeID { get; set; }
        public string? TypeName { get; set; }
        public int? Capacity { get; set; }
    }
}
