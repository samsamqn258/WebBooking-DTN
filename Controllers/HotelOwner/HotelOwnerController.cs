using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner
{
    public class HotelOwnerController : Controller
    {
        private UserI_Repository _userIRepository;
        public HotelOwnerController( UserI_Repository hotelOwnerRepository)
        { 
            _userIRepository = hotelOwnerRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayInfoND()
        {
            int? NguoiDungId = HttpContext.Session.GetInt32("NguoiDungId");
            if (!NguoiDungId.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var nguoiDung = await _userIRepository.GetByIdAsync(NguoiDungId.Value);
            return View(new List<User> { nguoiDung });
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerInfo(string fullName, string phoneNumber)
        {


            // Check if NguoiDungId is null
            int? NguoiDungId = HttpContext.Session.GetInt32("NguoiDungId");
            if (!NguoiDungId.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            // Kiểm tra xem dữ liệu đã được gửi chưa
            if (fullName != null && phoneNumber != null)
            {
                // Tạo đối tượng khách hàng với dữ liệu từ form

                var customer = await _nguoiDungI_Repository.GetByIdAsync(NguoiDungId.Value);

                customer.TenNguoiDung = fullName;
                customer.SoDienThoai = phoneNumber;

                await _nguoiDungI_Repository.UpdateAsync(customer);

                return RedirectToAction("DisplayInfoND", "NguoiDung");
            }
            else
            {
                // Nếu dữ liệu không đầy đủ, có thể chuyển hướng người dùng đến trang lỗi hoặc hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View(); // Hoặc chuyển hướng đến view của form
            }
        }
    }
}
