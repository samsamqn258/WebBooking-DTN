using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class HotelType
    {
        [Key]
        public int TypeID { get; set; }
        public string? TypeName { get; set; }
        public string? Image { get; set;}
    }
}
