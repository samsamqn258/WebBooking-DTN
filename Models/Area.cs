using System.ComponentModel.DataAnnotations;

namespace WebBooking.Models
{
    public class Area
    {
        [Key]

        public int AreaID { get; set; }
        public string? AreaName { get; set; }
        public string? Image { get; set; }
        public int NumberOfHotels { get; internal set; }
    }
}
