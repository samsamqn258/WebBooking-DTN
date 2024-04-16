namespace WebBooking.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string? RoomNumber { get; set; }
        public double? PricePerNight { get; set; }
        public string? Description { get; set; }
        public string? Image1 { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public bool IsActive { get; set; }
        public double Discount { get; set; }
        public int HotelID { get; set; }
        public int TypeID { get; set; }

        public Hotel? Hotel { get; set; }
        public RoomType? RoomType { get; set; }
    }
}
