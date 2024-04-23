using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;

namespace WebBooking.Controllers.HotelOwner.PROCESS
{
    public class ProcessBookingHotelOwnerController : Controller
    {
        private HotelI_Repository _hotelIRepository;
        private BookingI_Repository _bookingIRepository;
        public ProcessBookingHotelOwnerController(HotelI_Repository hotelRepository, BookingI_Repository bookingRepository)
        {
            _hotelIRepository = hotelRepository;
            _bookingIRepository = bookingRepository;

        }
        public IActionResult Index()
        {
            return View();
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

            if (hotel == null)
            {

                ViewBag.Message = "Khách sạn không tồn tại";
                return View();
            }

            var listBooking = await _bookingIRepository.ListYourBookingHotel(hotel);

            if (listBooking == null)
            {
                ViewBag.Message = "Khách sạn của bạn hiện không có phiếu đặt.";
                return View();
            }
            return View(listBooking);
        }
        public async Task<IActionResult> UpdateBooking()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> UpdateBooking(int bookingId)
        {
            var booking = await _bookingIRepository.GetByIdAsyncBooking(bookingId);
            if (booking == null)
            {
                ViewBag.Message = "Phiếu không tồn tại";
                return View();
            }
            booking.BookingStatus = true;
            await _bookingIRepository.UpdateAsync(booking);
            ViewBag.BookingId = bookingId;
            return RedirectToAction("ListBookingConfirm", "ProcessBookingHotelOwner");
        }
        public async Task<IActionResult> ListBookingConfirm()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }

            var hotel = await _hotelIRepository.GetByIdAsync(hotelOwnerInfo.Value);

            if (hotel == null)
            {

                ViewBag.Message = "Khách sạn không tồn tại";
                return View();
            }

            var listBooking = await _bookingIRepository.BookingConfirm(hotel);

            if (listBooking == null)
            {
                ViewBag.Message = "Khách sạn của bạn hiện không có phiếu đặt.";
                return View();
            }
            return View(listBooking);
        }

    }
}
