using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class PaymentEF_Repository: PaymentI_Repository
    {
        private readonly MyData _myData;

        public PaymentEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task AddAsync(Payment payment)
        {
            _myData.Payments.Add(payment);
            await _myData.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _myData.Payments.Update(payment);
            await _myData.SaveChangesAsync();
        }
        public async Task<IEnumerable<Payment>> GetAllByHotelOwnerIdAsync(int hotelOwnerId)
        {
            return await _myData.Payments
                .Include(ptt => ptt.Booking)
                    .ThenInclude(pdp => pdp.Room)
                        .ThenInclude(p => p.Hotel)
                .Where(ptt => ptt.Booking.Room.Hotel.UserID == hotelOwnerId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Payment>> GetAllByGuestIdAsync(int guestId)
        {
            return await _myData.Payments
                            .Where(ptt => ptt.Booking.UserID == guestId).Include(ptt => ptt.Booking).ThenInclude(ptt => ptt.Room).ThenInclude(ptt=>ptt.Hotel).ToListAsync();
        }
    }
}
