using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface AreaI_Repository
    {
        Task<IEnumerable<Area>> GetAllListArea();
        Task<IEnumerable<Area>> ListAreaFamous();
        Task<Area> GetAreaName(int AreaId);
    }
}
