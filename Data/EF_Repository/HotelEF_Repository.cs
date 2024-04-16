using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class HotelEF_Repository: HotelI_Repository
    {
        private readonly MyData _myData;

        public HotelEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _myData.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> GetAvailableHotelsAsync(DateTime fromDate, DateTime toDate, int numberOfGuests, string location)
        {
            List<Hotel> availableHotels = new List<Hotel>();

            // Lấy danh sách tất cả khách sạn có thể có
            var allHotels = await _myData.Hotels.Where(hotel => hotel.IsActive).Include(hotel => hotel.Area).ToListAsync();

            // Chuyển location và tên khu vực của khách sạn về dạng không dấu, chữ thường
            string normalizedLocation = RemoveAccents(location.ToLower());

            // Lặp qua từng khách sạn
            foreach (var hotel in allHotels)
            {
                // Chuyển tên khu vực của khách sạn về dạng không dấu, chữ thường
                string normalizedHotelLocation = RemoveAccents(hotel.Area.AreaName.ToLower());

                // Kiểm tra địa chỉ nếu có
                if (!string.IsNullOrEmpty(location) && !normalizedHotelLocation.Contains(normalizedLocation, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Không khớp địa chỉ, bỏ qua khách sạn này
                }

                // Lấy danh sách tất cả phòng thuộc khách sạn đang xét
                var rooms = await _myData.Rooms
                    .Where(room => room.HotelID == hotel.HotelID && room.IsActive)
                    .Include(room => room.RoomType)
                    .ToListAsync();

                // Kiểm tra từng phòng
                foreach (var room in rooms)
                {
                    // Kiểm tra sức chứa và sự sẵn có của phòng
                    if (room.RoomType.Capacity == numberOfGuests &&
                        !await IsRoomBookedAsync(room.RoomID, fromDate, toDate))
                    {
                        availableHotels.Add(hotel); // Phòng có sẵn, thêm vào danh sách kết quả
                        break; // Thoát vòng lặp, kiểm tra khách sạn tiếp theo
                    }
                }
            }

            return availableHotels;
        }

        private string RemoveAccents(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormKD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormKC);
        }

        private async Task<bool> IsRoomBookedAsync(int roomId, DateTime fromDate, DateTime toDate)
        {
            // Kiểm tra xem phòng đã được đặt trong khoảng thời gian yêu cầu chưa
            return await _myData.Bookings
                .AnyAsync(booking =>
                    booking.RoomID == roomId &&
                    !(booking.CheckOutDate <= fromDate || booking.CheckInDate >= toDate) &&
                    !_myData.CancelBookings.Any(cancellation => cancellation.BookingID == booking.BookingID)

                );
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsByHotelIdAsync(int hotelId, DateTime fromDate, DateTime toDate, int numberOfGuests)
        {
            var availableRoomsQuery = _myData.Rooms.Include(hotel => hotel.Hotel)
                .Where(room =>
                    room.HotelID == hotelId &&
                    room.IsActive == true && // Phòng phải ở trạng thái hoạt động
                    !_myData.Bookings.Any(booking =>
                        booking.RoomID == room.RoomID &&
                        !(booking.CheckOutDate <= fromDate || booking.CheckInDate >= toDate) &&
                        !_myData.CancelBookings.Any(cancellation => cancellation.BookingID == booking.BookingID)


                    )
                );

            // Nếu có số lượng khách được chỉ định, lọc phòng theo số lượng khách
            availableRoomsQuery = availableRoomsQuery.Where(room => room.RoomType.Capacity >= numberOfGuests);

            var availableRooms = await availableRoomsQuery.ToListAsync();

            return availableRooms;
        }


        public async Task<Hotel> GetByIdAsync(int hotelOwnerID)
        {
            return await _myData.Hotels.FirstOrDefaultAsync(s => s.UserID == hotelOwnerID);
        }
        public async Task AddAsync(Hotel hotel)
        {

            _myData.Hotels.Add(hotel);
            await _myData.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {

            _myData.Hotels.Update(hotel);
            await _myData.SaveChangesAsync();
        }


        public async Task<Hotel> GetHotelName(int roomId)
        {
            var rooms = await _myData.Rooms
                .Include(p => p.Hotel) // Kết nối với bảng KhachSan
                .FirstOrDefaultAsync(p => p.RoomID == roomId);

            if (rooms != null)
            {
                return rooms.Hotel; // Trả về đối tượng KhachSan
            }
            return null;

        }

    }
}
