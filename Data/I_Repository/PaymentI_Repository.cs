using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface PaymentI_Repository
    {
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task<IEnumerable<Payment>> GetAllByHotelOwnerIdAsync(int hotelOwnerId);
        Task<IEnumerable<Payment>> GetAllByGuestIdAsync(int guestId);
    }
}
