using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class HotelTypeEF_Repository: HotelTypeI_Repository
    {
        private readonly MyData _myData;

        public HotelTypeEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<HotelType>> GetAllListHotelType()
        {
            return await _myData.HotelTypes.ToListAsync();
        }

        public async Task<HotelType> GetHotelTypeName(int TypeId)
        {
            return await _myData.HotelTypes.FirstOrDefaultAsync(a => a.TypeID == TypeId);
        }
    }
}
