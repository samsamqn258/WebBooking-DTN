using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class CancelBookingController : Controller
    {
        private CancelBookingI_Repository _cancelBookingIRepository;
        private BookingI_Repository _bookingIRepository;
 
        public CancelBookingController(BookingI_Repository bookingRepository, CancelBookingI_Repository cancelBookingIRepository)
        {

            _bookingIRepository = bookingRepository;
   
            _cancelBookingIRepository = cancelBookingIRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CancelNewBooking(int bookingId)
        {
            var checkBooking = await _bookingIRepository.GetByIdAsync(bookingId);

            if (checkBooking == null)
            {
                ViewBag.NoBooking = "Phiếu đặt phòng không tồn tại";
            }
            CancelBooking cancelForm = new CancelBooking();
            cancelForm.BookingID = bookingId;
            cancelForm.CancelTime = DateTime.Now;

            // Cập nhật phiếu đặt phòng trong cơ sở dữ liệu
            await _cancelBookingIRepository.AddAsync(cancelForm);

            // Chuyển hướng người dùng đến trang danh sách các đặt phòng của họ
            return RedirectToAction("ListYourBookingForm", "CreateBooking");
        }

        public async Task<IActionResult> ListYourCancelBookingForm()
        {
            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var listCancelBooking = await _cancelBookingIRepository.GetListCancelBooking(guestInfo.Value);
            if (listCancelBooking != null && listCancelBooking.Any())
            {
                return View(listCancelBooking);
            }
            else
            {
                ViewBag.NoBooking = "Không có phòng nào đã hủy";
                return View();
            }
           
        }

    }
}
