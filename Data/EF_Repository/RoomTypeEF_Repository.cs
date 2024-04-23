using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class RoomTypeEF_Repository: RoomTypeI_Repository
    {
        private readonly MyData _myData;

        public RoomTypeEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<RoomType>> GetAllListRoomType()
        {
            return await _myData.RoomTypes.ToListAsync();
        }
    }
}
