using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class BookingEF_Repository: BookingI_Repository
    {
        private readonly MyData _myData;

        public BookingEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task AddAsync(Booking booking)
        {
            _myData.Bookings.Add(booking);
            await _myData.SaveChangesAsync();
        }
        public async Task<Booking> GetByIdAsync(int guestId)
        {
            return _myData.Bookings.FirstOrDefault(p => p.UserID == guestId);
        }

        public async Task<Booking> GetByIdAsyncBooking(int bookingId)
        {
            return _myData.Bookings.FirstOrDefault(p => p.BookingID == bookingId);
        }

        public async Task<IEnumerable<Booking>> ListYourBookingForm(int guestId)
        {
            var unpaidBookings = await _myData.Bookings
                .Where(pdp => pdp.UserID == guestId && !_myData.CancelBookings.Any(phdp => pdp.BookingID == phdp.BookingID)).Include(pdp => pdp.Room).ThenInclude(pdp => pdp.Hotel)
                .ToListAsync();

            return unpaidBookings;
        }
        public async Task<List<Booking>> BookingConfirm(Hotel hotels)
        {
            var bookings = await (from pdp in _myData.Bookings
                                  join p in _myData.Rooms on pdp.RoomID equals p.RoomID
                                  where p.HotelID == hotels.HotelID
                                  && !_myData.CancelBookings.Any(phdp => phdp.BookingID == pdp.BookingID)
                                  && pdp.BookingStatus == true 
                                  select pdp).ToListAsync();


            return bookings;
        }
        public async Task<List<Booking>> ListYourBookingHotel(Hotel hotels)
        {
            var bookings = await (from pdp in _myData.Bookings
                                  join p in _myData.Rooms on pdp.RoomID equals p.RoomID
                                  where p.HotelID == hotels.HotelID
                                  && !_myData.CancelBookings.Any(phdp => phdp.BookingID == pdp.BookingID)
                                  && pdp.BookingStatus == false
                                  select pdp).ToListAsync();

            return bookings;
        }

        public async Task<string> GetNameHotel(int hotelId)
        {
            var nameHotel = await (from pdp in _myData.Bookings
                                     join p in _myData.Rooms on pdp.RoomID equals p.RoomID
                                     join ks in _myData.Hotels on p.HotelID equals ks.HotelID
                                     where pdp.BookingID == hotelId
                                   select ks.HotelName).FirstOrDefaultAsync();

            return nameHotel;
        }
        //public async Task<string> GetImage(int id)
        //{
        //    var hinhanh = await (from pdp in _myData.PhieuDatPhong
        //                         join p in _myData.Phong on pdp.PhongId equals p.Id
        //                         join ks in _myData.KhachSan on p.KhachSanId equals ks.Id
        //                         where pdp.Id == id
        //                         select ks.HinhAnh).FirstOrDefaultAsync();

        //    return hinhanh;
        //}

        public async Task UpdateAsync(Booking booking)
        {
            _myData.Bookings.Update(booking);
            await _myData.SaveChangesAsync();
        }
    }
}
