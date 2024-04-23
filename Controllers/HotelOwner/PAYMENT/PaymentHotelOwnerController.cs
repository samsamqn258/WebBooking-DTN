using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner.PAYMENT
{
    public class PaymentHotelOwnerController : Controller
    {
        private BookingI_Repository _bookingIRepository;
        private PaymentI_Repository _paymentIRepository;
        public PaymentHotelOwnerController(BookingI_Repository bookingRepository, PaymentI_Repository paymentRepository)
        {

            _bookingIRepository = bookingRepository;
            _paymentIRepository = paymentRepository;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListBillHotelOwner()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var bill = await _paymentIRepository.GetAllByHotelOwnerIdAsync(hotelOwnerInfo.Value);
            if (bill != null && bill.Any())
            {
                return View(bill);
            }
            else
            {
                ViewBag.NoBill = "Không có hóa đơn nào";
                return View();
            }
        }
        public async Task<IActionResult> CashPayment(int bookingId)
        {
            var booking = await _bookingIRepository.GetByIdAsyncBooking(bookingId);
            if (booking == null)
            {
                ViewBag.NoBooking = "Phiếu đặt phòng không tồn tại";
            }
            Payment bill = new Payment();
            bill.TypeID = 2;
            bill.BookingID = booking.BookingID;
            bill.PaymentDate = DateTime.Now;
            bill.TotalPayment = booking.TotalPriceBooking;

            await _paymentIRepository.AddAsync(bill);
            booking.PaymentStatus = true;
            await _bookingIRepository.UpdateAsync(booking);

            ViewBag.ok = "Thanh toán thành công";
            return RedirectToAction("ListBillHotelOwner", "PaymentHotelOwner");

        }
    }
}
