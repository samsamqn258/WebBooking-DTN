using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner.ACCOUNT
{
    public class AccountHotelOwnerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private UserI_Repository _userI_Repository;
        public AccountHotelOwnerController(UserI_Repository userI_Repository)
        {
            _userI_Repository = userI_Repository;
        }


        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public async Task<IActionResult> RegisterHotelOwner()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterHotelOwner(string hotelOwnerName, string email, string password)
        {
            if (string.IsNullOrEmpty(hotelOwnerName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin";
            }

            if (IsValidEmail(email))
            {
                var checkEmail = await _userI_Repository.CheckEmail(email);
                if (checkEmail == null)
                {
                    password = BCrypt.Net.BCrypt.HashPassword(password);

                    User hotelOwner = new User { UserName = hotelOwnerName, Email = email, Password = password, UserType = 1 };

                    // Thêm người dùng mới vào cơ sở dữ liệu
                    await _userI_Repository.AddAsync(hotelOwner);
                    ViewBag.ErrorMessage = "Đăng ký thành công";
                    return RedirectToAction("LoginHotelOwner", "AccountHotelOwner");
                }
                else
                {
                    ViewBag.ErrorMessage = "Email này được sử dụng. Vui lòng chọn một địa chỉ email khác.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Email không hợp lệ";
                return View();
            }
        }

        public async Task<IActionResult> LoginHotelOwner(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            if (IsValidEmail(email))
            {
                var hotelOwner = await _userI_Repository.CheckEmail(email);

                if (hotelOwner != null && BCrypt.Net.BCrypt.Verify(password, hotelOwner.Password
                    ) && hotelOwner.UserType == 1)
                {

                    // Lưu ID của người dùng vào biến session
                    HttpContext.Session.SetInt32("UserID", hotelOwner.UserID);
                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Email, hotelOwner.Email),
                      new Claim(ClaimTypes.Name, hotelOwner.UserName),
                      new Claim(ClaimTypes.Role, hotelOwner.UserType.ToString()),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                    };
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Index", "Hotel");
                }
                else
                {
                    ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng.";
                    return View();
                }

            }
            else
            {
                ViewBag.ErrorMessage = "Email không hợp lệ.";
                return View();
            }
        }

        public async Task<IActionResult> LogOutHotelOwner()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Xóa tất cả các key-value pairs trong session của khách hàng
            return RedirectToAction("Index", "HotelOwner");
        }
    }
}
