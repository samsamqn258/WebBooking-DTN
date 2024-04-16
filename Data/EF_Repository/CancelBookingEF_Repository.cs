using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class CancelBookingEF_Repository: CancelBookingI_Repository
    {
        private readonly MyData _myData;

        public CancelBookingEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<CancelBooking>> GetListCancelBooking(int guestId)
        {
            var cancelBooking = await _myData.CancelBookings
                                           .Where(phdp => phdp.Booking.UserID == guestId).Include(phdp => phdp.Booking).ThenInclude(phdp => phdp.Room).ThenInclude(phdp => phdp.Hotel)
                                           .ToListAsync();

            return cancelBooking;
        }
        public async Task AddAsync(CancelBooking cancelBooking)
        {
            _myData.CancelBookings.Add(cancelBooking);
            await _myData.SaveChangesAsync();
        }

    }
}
