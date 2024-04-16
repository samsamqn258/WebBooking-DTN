using Microsoft.AspNetCore.Mvc;
using WebBooking.Data;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class GuestController : Controller
    {
    
        private HotelI_Repository _hotelIRepository;
        private UserI_Repository _userIRepository;
        public GuestController(HotelI_Repository hotelRepository, UserI_Repository guestRepository)
        {
            _hotelIRepository = hotelRepository;
            _userIRepository = guestRepository;

        }
        public async Task<IActionResult> Index()
        {
            var allHotels = await _hotelIRepository.GetAllAsync();
            return View(allHotels);
        }

        public async Task<IActionResult> DisplayInfoGuest()
        {
            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var guest = await _userIRepository.GetByIdAsync(guestInfo.Value);
            return View(new List<User> { guest });
        }
        [HttpPost]
        public async Task<IActionResult> SaveGuestInfo(string guestName, string phoneNumber)
        {

            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            // Kiểm tra xem dữ liệu đã được gửi chưa
            if (guestName != null && phoneNumber != null)
            { 
                var guest = await _userIRepository.GetByIdAsync(guestInfo.Value);
                guest.UserName = guestName;
                guest.PhoneNumber = phoneNumber;
              
                await _userIRepository.UpdateAsync(guest);
                return RedirectToAction("DisplayInfoGuest", "Guest");
            }
            else
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View(); 
            }
        }
    }
}
