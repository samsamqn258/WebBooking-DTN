using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner.INFO
{
    public class HotelOwnerController : Controller
    {
        private UserI_Repository _userIRepository;
        public HotelOwnerController(UserI_Repository hotelOwnerRepository)
        {
            _userIRepository = hotelOwnerRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayInfoHotelOwner()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Tài khoản không tồn tại";
                return View();
            }
            var hotelOwner = await _userIRepository.GetByIdAsync(hotelOwnerInfo.Value);
            return View(new List<User> { hotelOwner });
        }

        [HttpPost]
        public async Task<IActionResult> SaveHotelOwnerInfo(string hotelOwnerName, string phoneNumber)
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
                ViewBag.NoGuest = "Tài khoản không tồn tại";
                return View();
            }
            // Kiểm tra xem dữ liệu đã được gửi chưa
            if (hotelOwnerInfo != null && phoneNumber != null)
            {
                var hotelOwner = await _userIRepository.GetByIdAsync(hotelOwnerInfo.Value);
                hotelOwner.UserName = hotelOwnerName;
                hotelOwner.PhoneNumber = phoneNumber;

                await _userIRepository.UpdateAsync(hotelOwner);
                return RedirectToAction("DisplayInfoHotelOwner", "Guest");
            }
            else
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }
        }
    }
}
