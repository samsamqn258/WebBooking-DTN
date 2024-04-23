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
        Task<IEnumerable<Hotel>> GetListHotelOwner(int hotelOwnerID);
        Task <Hotel> GetValueHotel (int hotelId);
        Task<Hotel> FindHotel(int hotelId);
        Task<IEnumerable<Hotel>> ListHotelFamous();
        Task<IEnumerable<Hotel>> ListHotelInArea(int AreaId);
        Task<IEnumerable<Hotel>> ListHoteltype(int TypeId);
    }
}
