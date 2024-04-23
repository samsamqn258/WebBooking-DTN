using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface RoomTypeI_Repository
    {
        Task<IEnumerable<RoomType>> GetAllListRoomType();
    }
}
