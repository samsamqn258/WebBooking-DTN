using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface CancelBookingI_Repository
    {
        Task AddAsync(CancelBooking cancelBooking);
        Task<IEnumerable<CancelBooking>> GetListCancelBooking(int guestId);
    }
}
