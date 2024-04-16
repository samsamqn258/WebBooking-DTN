using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class CreateBookingController : Controller
    {
        private HotelI_Repository _hotelIRepository;
        private UserI_Repository _userIRepository;
        private BookingI_Repository _bookingIRepository;
        private RoomI_Repository _roomIRepository;
        public CreateBookingController(HotelI_Repository hotelRepository, UserI_Repository guestRepository, BookingI_Repository bookingRepository, RoomI_Repository roomRepository)
        {
            _hotelIRepository = hotelRepository;
            _userIRepository = guestRepository;
            _bookingIRepository = bookingRepository;
            _roomIRepository = roomRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateNewBooking(int roomId, DateTime fromDate, DateTime toDate)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;

            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            // Kiểm tra nếu fromDate và toDate là giá trị nhỏ nhất và numberOfGuests là 0 hoặc rỗng
            if (fromDate == DateTime.MinValue && toDate == DateTime.MinValue)
            {
                ViewBag.InvalidBookingInfo = "Thông tin đặt phòng không hợp lệ.";
                return View();
            }
            TimeSpan rentalTime = toDate.Date.Subtract(fromDate.Date);
            int daysToRent = (int)rentalTime.TotalDays;
            // Lấy thông tin khách hàng từ repository
            var guest = await _userIRepository.GetByIdAsync(guestInfo.Value);

            var room = await _roomIRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                ViewBag.NoPhong = "Phòng không tồn tại";
                return View();
            }

            // Kiểm tra xem giá của phòng có null không
            if (room.PricePerNight == null)
            {
                ViewBag.NoPrice = "Giá phòng không được đặt";
                return View();
            }

            // Tính giá phòng dựa trên giá mỗi đêm và số ngày thuê
            double priceOfRoom = (double)room.PricePerNight * daysToRent;

            // Áp dụng khuyến mãi nếu có
            if (room.Discount != null && room.Discount > 0)
            {
                priceOfRoom -= priceOfRoom * (double)room.Discount / 100; // Giảm giá theo phần trăm khuyến mãi
            }

            // Tạo phiếu đặt phòng
            var booking = new Booking
            {
                RoomID = roomId,
                UserID = guest.UserID,
                CheckInDate = fromDate,
                CheckOutDate = toDate,
                BookingTime = DateTime.Now,
                BookingStatus = false,
                TotalPriceBooking = priceOfRoom,
            };

            // Lưu phiếu đặt phòng vào database
            await _bookingIRepository.AddAsync(booking);

            return RedirectToAction("Index", "Guest");
        }
        public async Task<IActionResult> ListYourBookingForm()
        {
            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var guest = await _userIRepository.GetByIdAsync(guestInfo.Value);
            if (guest == null)
            {
                ViewBag.NoGuest = "Khách hàng không tồn tại";
            }
            var listBookingGuest = await _bookingIRepository.ListYourBookingForm(guestInfo.Value);
            if (listBookingGuest == null || !listBookingGuest.Any()) // Kiểm tra xem danh sách có trống không
            {
                ViewBag.NoFrom = "Danh sách đặt phòng của bạn đang trống";
                return View(); // Trả về view mà không có dữ liệu
            }
            return View(listBookingGuest);
        }

    }
}
