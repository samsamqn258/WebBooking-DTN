using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface HotelI_Repository
    {
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task<IEnumerable<Hotel>> GetAvailableHotelsAsync(DateTime fromDate, DateTime toDate, int numberOfGuests, String location);
        Task<Hotel> GetByIdAsync(int hotelOwnerID);
        Task AddAsync(Hotel hotel);
        Task<Hotel> GetHotelName(int roomId);
        Task<IEnumerable<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, DateTime fromDate, DateTime toDate, int numberOfGuests);
        Task UpdateAsync(Hotel hotel);

    }
}
