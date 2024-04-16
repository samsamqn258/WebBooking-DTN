namespace WebBooking.Models
{
    public class FavoriteHotel
    {
        public int HotelID { get; set; }
        public int UserID { get; set; }

        public Hotel? Hotel { get; set; }
        public User? User { get; set; }
    }
}
