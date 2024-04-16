using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface RoomI_Repository
    {
        Task<Room> GetByIdAsync(int roomId);
        Task AddAsync(Room room);
        Task UpdateAsync(Room room);
        Task<IEnumerable<Room>> GetAllByAllRoomInHotelIdAsync(int hotelId);
    }
}
