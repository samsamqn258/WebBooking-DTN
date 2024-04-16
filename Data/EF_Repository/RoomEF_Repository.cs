using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class RoomEF_Repository: RoomI_Repository
    {
        private readonly MyData _myData;

        public RoomEF_Repository(MyData myData)
        {
            _myData = myData;
        }

        public async Task AddAsync(Room room)
        {
            _myData.Rooms.Add(room);
            await _myData.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _myData.Rooms.Update(room);
            await _myData.SaveChangesAsync();
        }

        public async Task<Room> GetByIdAsync(int roomId)
        {
            return await _myData.Rooms.FirstOrDefaultAsync(p => p.RoomID == roomId);
        }

        public async Task<IEnumerable<Room>> GetAllByAllRoomInHotelIdAsync(int hotelId)
        {
            return await _myData.Rooms.Where(p => p.HotelID == hotelId).ToListAsync();
        }


    }
}
