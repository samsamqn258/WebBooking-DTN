using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class AreaEF_Repository : AreaI_Repository
    {
        private readonly MyData _myData;

        public AreaEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<Area>> GetAllListArea()
        {
            return await _myData.Areas.ToListAsync();
        }
        public async Task<IEnumerable<Area>> ListAreaFamous()
        {
            // Sử dụng LINQ để thực hiện truy vấn
            var famousAreas = await _myData.Hotels.Where(h => h.IsActive == true)
                .GroupBy(h => new { h.AreaID, h.Area.AreaName, h.Area.Image })
                .Select(g => new Area
                {
                    AreaID = g.Key.AreaID,
                    AreaName = g.Key.AreaName,
                    Image = g.Key.Image,
                    NumberOfHotels = g.Count()
                })
                .OrderByDescending(a => a.NumberOfHotels)
                .Take(10)
                .ToListAsync();


            return famousAreas;
        }
        public async Task<Area> GetAreaName(int AreaId)
        {
            return await _myData.Areas.FirstOrDefaultAsync(a => a.AreaID == AreaId);
        }
    }
}
