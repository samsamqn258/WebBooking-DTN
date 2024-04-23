using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;

namespace WebBooking.Controllers.HotelOwner.LISTBOOKING
{
    public class ListFormController : Controller
    {
        private HotelI_Repository _hotelIRepository;
        private BookingI_Repository _bookingIRepository;
        private PaymentI_Repository _paymentIRepository;
        private UserI_Repository _userI_Repository;
        public ListFormController(HotelI_Repository hotelRepository, BookingI_Repository bookingRepository, PaymentI_Repository paymentIRepository, UserI_Repository userI_Repository)
        {
            _hotelIRepository = hotelRepository;
            _bookingIRepository = bookingRepository;
            _paymentIRepository = paymentIRepository;
            _userI_Repository = userI_Repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListPayment()
        {
     
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }


            // Lấy danh sách phiếu thanh toán
            var listPayment = await _paymentIRepository.GetAllByHotelOwnerIdAsync(hotelOwnerInfo.Value);

            // Xử lý trường hợp không có phiếu thanh toán
            if (listPayment == null || !listPayment.Any())
            {
                ViewBag.NoBill = "Khách sạn của bạn hiện không có phiếu thanh toán.";
                return View();
            }

            // Truyền danh sách phiếu thanh toán cho trang View
            return View(listPayment);
        }
        public async Task<IActionResult> ListBookingInHotel()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }

            var hotel = await _hotelIRepository.GetByIdAsync(hotelOwnerInfo.Value);

            var listBooking = await _bookingIRepository.ListYourBookingHotel(hotel);

            if (listBooking == null )
            {
                ViewBag.NoBooking = "Khách sạn của bạn hiện không có phiếu đặt.";
                
            }
            return View(listBooking);
        }
        public async Task<IActionResult> ListBookingConfirmed()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }

            var hotel = await _hotelIRepository.GetByIdAsync(hotelOwnerInfo.Value);

            var listBookingConfirmed = await _bookingIRepository.BookingConfirm(hotel);

            if (listBookingConfirmed == null)
            {
                ViewBag.NoBookingConfirmed = "Khách sạn của bạn hiện không có phiếu đặt.";
                return View();
            }
            return View(listBookingConfirmed);
        }
    }
}
