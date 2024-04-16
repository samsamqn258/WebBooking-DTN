using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WebBooking.Data;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class AccountGuestController : Controller
    {
    
        private UserI_Repository _userI_Repository;
        public AccountGuestController( UserI_Repository userI_Repository)
        {
            _userI_Repository = userI_Repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        private bool IsValidEmail(string email)
        {
            // Biểu thức chính quy để kiểm tra định dạng email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public async Task<IActionResult> RegisterGuest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterGuest( string guestName, string email, string password)
        {
            if (string.IsNullOrEmpty(guestName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin";
            }

            if (IsValidEmail(email))
            {
                var checkEmail = await _userI_Repository.CheckEmail(email);
                if (checkEmail == null)
                {
                    password = BCrypt.Net.BCrypt.HashPassword(password);

                    User guest = new User { UserName = guestName, Email = email, Password = password, UserType = 2 };

                    // Thêm người dùng mới vào cơ sở dữ liệu
                    await _userI_Repository.AddAsync(guest);
                    ViewBag.ErrorMessage = "Đăng ký thành công";
                    return RedirectToAction("LoginGuest", "AccountGuest");
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

        public async Task<IActionResult> LoginGuest(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            if (IsValidEmail(email))
            {
                var guest = await _userI_Repository.CheckEmail(email);

                if (guest != null && BCrypt.Net.BCrypt.Verify(password, guest.Password
                    ) && guest.UserType == 2)
                {

                    // Lưu ID của người dùng vào biến session
                    HttpContext.Session.SetInt32("UserID", guest.UserID);
                    var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Email, guest.Email),
                      new Claim(ClaimTypes.Name, guest.UserName),
                      new Claim(ClaimTypes.Role, guest.UserType.ToString()),
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
                    return RedirectToAction("Index", "Guest");
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
        public async Task LoginGoogle()
        {
            // Xóa phiên làm việc hiện tại
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng đến quá trình xác thực Google với prompt=select_account
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleReponse"),
                Parameters =
            {
            { "prompt", "select_account" }
            }
            });
        }
        public async Task<IActionResult> GoogleReponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Claims.ToList();

            // Lấy thông tin từ Google Claims
            // Lưu ID của người dùng vào biến session

            var guestName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var password = claims.FirstOrDefault(x => x.Type == "urn:google:password")?.Value;

            if (string.IsNullOrEmpty(guestName) || string.IsNullOrEmpty(email))
            {
                // Trường hợp không tìm thấy thông tin cần thiết từ Google Claims
                // Xử lý lỗi tại đây, ví dụ chuyển hướng đến trang lỗi
                return RedirectToAction("Error", "KhachHang");
            }
            // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu chưa
            var checkGuest = await _userI_Repository.CheckEmail(email);
            if (checkGuest == null)
            {
                var guest = await _userI_Repository.CheckEmail(email);
                HttpContext.Session.SetInt32("UserID", guest.UserID);
                // Nếu tài khoản không tồn tại, thêm tài khoản mới vào cơ sở dữ liệu
                // Mã hóa mật khẩu từ Google trước khi lưu vào cơ sở dữ liệu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password ?? "YourGeneratedPassword");
                User guestnNew = new User { UserName = guestName, Email = email, Password = password, UserType = 2 };
                // Thêm người dùng mới vào cơ sở dữ liệu
                await _userI_Repository.AddAsync(guestnNew);//u thay đổi vào cơ sở dữ liệu
                                                  // Thêm các Claims vào principal
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, guestName),
            new Claim(ClaimTypes.Role, "2")
            // Thêm các Claims khác nếu cần
        }, CookieAuthenticationDefaults.AuthenticationScheme);

                // Đăng nhập bằng thông tin từ Google
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                ViewBag.SuccessMessage = "Đăng ký thành công!";
            }
            else
            {
                var guest = await _userI_Repository.CheckEmail(email);
                HttpContext.Session.SetInt32("UserID", guest.UserID);
                // Nếu tài khoản đã tồn tại, cập nhật mật khẩu từ Google vào cơ sở dữ liệu// Mã hóa mật khẩu từ Google trước khi cập nhật vào cơ sở dữ liệu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password ?? "YourGeneratedPassword");
                User guestnNew = new User { UserName = guestName, Email = email, Password = password, UserType = 2 }; ;

                // Thêm các Claims vào principal
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, guestName),
            new Claim(ClaimTypes.Role, "2")
            // Thêm các Claims khác nếu cần
        }, CookieAuthenticationDefaults.AuthenticationScheme);

                // Đăng nhập bằng thông tin từ Google
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                ViewBag.SuccessMessage = "Đăng nhập thành công!";
            }
            // Sau khi thêm tài khoản mới hoặc xử lý tài khoản hiện có, chuyển hướng đến trang chính
            return RedirectToAction("Index", "KhachHang");
        }
        public async Task<IActionResult> LogOutGuest()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear(); // Xóa tất cả các key-value pairs trong session của khách hàng
            return RedirectToAction("Index", "Guest");
        }
    }
}
