using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface BookingI_Repository
    {
        Task AddAsync(Booking booking);
        Task<Booking> GetByIdAsync(int guestId);
        Task<Booking> GetByIdAsyncBooking(int bookingId);

        Task<IEnumerable<Booking>> ListYourBookingForm(int guestId);
        Task<string> GetNameHotel(int hotelId);
        //Task<string> GetImage(int hotelId);

        Task UpdateAsync(Booking booking);
        Task<List<Booking>> BookingConfirm(Hotel hotels);
        Task<List<Booking>> ListYourBookingHotel(Hotel hotels);

    }
}
