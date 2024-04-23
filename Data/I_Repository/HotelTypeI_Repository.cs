using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface HotelTypeI_Repository
    {
        Task<IEnumerable<HotelType>> GetAllListHotelType();
        Task<HotelType> GetHotelTypeName(int TypeId);
    }
}
