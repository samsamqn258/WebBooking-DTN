using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBooking.Data;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner.HOTEL
{
    public class HotelController : Controller
    {

        private HotelI_Repository _hotelIRepository;
        private BookingI_Repository _bookingIRepository;
        private RoomI_Repository _roomIRepository;
        private CancelBookingI_Repository _cancelBookingIRepository;
        private UserI_Repository _userI_Repository;
        private AreaI_Repository    _areaIRepository;
        private HotelTypeI_Repository _hotelTypeIRepository;

        public HotelController(HotelI_Repository hotelI_Repository, BookingI_Repository bookingI_Repository, RoomI_Repository roomI_Repository, CancelBookingI_Repository cancelBookingI_Repository, UserI_Repository userI_Repository, AreaI_Repository areaI_Repository, HotelTypeI_Repository hotelTypeIRepository)
        {
            _hotelIRepository = hotelI_Repository;
            _bookingIRepository = bookingI_Repository;
            _roomIRepository = roomI_Repository;
            _cancelBookingIRepository = cancelBookingI_Repository;
            _userI_Repository = userI_Repository;
            _areaIRepository = areaI_Repository;
            _hotelTypeIRepository = hotelTypeIRepository;
        }

        public async Task<IActionResult> Index()
        {
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");
            if (!hotelOwnerInfo.HasValue)
            {
           
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }

            // Lấy danh sách khách sạn của người dùng hiện tại từ DbContext
            var hotel = await _hotelIRepository.GetListHotelOwner(hotelOwnerInfo.Value);

            if (hotel == null && hotel.Any())
            {
                ViewBag.NoHotel = "Hiện tại bạn chưa có khách sạn";
                return View();
            }

            return View(hotel);

        }

        public async Task<IActionResult> Details(int hotelId)
        {
            if (hotelId == null)
            {
                return NotFound();
            }

            var hotelDetail = await _hotelIRepository.GetValueHotel(hotelId);

            if (hotelDetail == null)
            {
                ViewBag.NoHotel = "Hiện tại bạn chưa có khách sạn";
                return View();
            }

            return View(hotelDetail);
        }

        public async Task<IActionResult> Create()
        {
            // Lấy ID của người dùng từ session
            int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");

            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!hotelOwnerInfo.HasValue)
            {
                // Nếu không có người dùng đăng nhập, hiển thị thông báo lỗi
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }

            // Người dùng đã đăng nhập, gán ID của họ vào ViewData để sử dụng trong view
            var areas = await _areaIRepository.GetAllListArea();
            var hotelTypes = await _hotelTypeIRepository.GetAllListHotelType();
            ViewData["UserID"] = hotelOwnerInfo.Value;
            ViewData["AreaID"] = new SelectList(areas, "AreaID", "AreaName");
            ViewData["TypeID"] = new SelectList(hotelTypes, "TypeID", "TypeName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelID,TypeID,HotelName,PhoneNumber,AreaID,Address,Image,IsActive")] Hotel hotel, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            if (ModelState.IsValid)
            {
                // Lấy ID của người dùng từ session
                int? hotelOwnerInfo = HttpContext.Session.GetInt32("UserID");

                // Kiểm tra xem người dùng đã đăng nhập hay chưa
                if (!hotelOwnerInfo.HasValue)
                {
                    // Nếu không có người dùng đăng nhập, hiển thị thông báo lỗi
                    ViewBag.NoGuest = "Khách hàng không tồn tại";
                    return View();
                }

                // Gán ID của người dùng vào đối tượng khách sạn
                hotel.UserID = hotelOwnerInfo.Value;

                // Kiểm tra xem người dùng đã có khách sạn chưa
                var existingHotel = await _hotelIRepository.GetByIdAsync(hotelOwnerInfo.Value);
                if (existingHotel != null)
                {
                    // Người dùng đã có khách sạn, bạn có thể thực hiện các hành động cần thiết như thông báo lỗi hoặc chuyển hướng đến một trang khác.
                    ModelState.AddModelError(string.Empty, "Người dùng đã tạo khách sạn.");
                    ModelState.Clear();
                    var emptyHotel = new Hotel();
                    var areas = await _areaIRepository.GetAllListArea();
                    var hotelTypes = await _hotelTypeIRepository.GetAllListHotelType();
                    ViewData["UserID"] = hotelOwnerInfo.Value;
                    ViewData["AreaID"] = new SelectList(areas, "AreaID", "AreaName", hotel.AreaID);
                    ViewData["TypeID"] = new SelectList(hotelTypes, "TypeID", "TypeName", hotel.TypeID);
                    return View(emptyHotel);
                }

                // Lưu hình ảnh nếu có
                if (image1 != null)
                {
                    hotel.Image1 = await SaveImage(image1);
                }
                if (image2 != null)
                {
                    hotel.Image2 = await SaveImage(image2);
                }
                if (image3 != null)
                {
                    hotel.Image3 = await SaveImage(image3);
                }

                // Lưu thông tin khách sạn vào cơ sở dữ liệu
                await _hotelIRepository.AddAsync(hotel);
                return RedirectToAction(nameof(Index));
            }   
            return View(hotel);
              
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine("wwwroot/img", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/img/" + fileName;
        }

        public async Task<IActionResult> Edit(int hotelId)
        {
            var hotel = await _hotelIRepository.GetValueHotel(hotelId);
            if (hotel == null)
            {
                return NotFound();
            }
            var areas = await _areaIRepository.GetAllListArea();
            var hotelTypes = await _hotelTypeIRepository.GetAllListHotelType();
            ViewData["AreaID"] = new SelectList(areas, "AreaID", "AreaName", hotel.AreaID);
            ViewData["TypeID"] = new SelectList(hotelTypes, "TypeID", "TypeName", hotel.TypeID);

            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int hotelId, [Bind("HotelID,TypeID,HotelName,PhoneNumber,AreaID,Address,Image,IsActive")] Hotel hotel, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            // Loại bỏ xác thực ModelState cho ImageUrl
            ModelState.Remove("Image1");
            ModelState.Remove("Image2");
            ModelState.Remove("Image3");

            if (hotelId != hotel.HotelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Tìm khách sạn hiện có trong cơ sở dữ liệu
                var existingHotel = await _hotelIRepository.FindHotel(hotelId);
                if (existingHotel == null)
                {
                    return NotFound(); // Trả về NotFound nếu không tìm thấy đối tượng với id tương ứng
                }

                // Cập nhật các thuộc tính của existingKhachSan từ khachSan
                existingHotel.TypeID = hotel.TypeID;
                existingHotel.AreaID = hotel.AreaID;
                existingHotel.HotelName = hotel.HotelName;
                existingHotel.Address = hotel.Address;
                existingHotel.PhoneNumber = hotel.PhoneNumber;
                existingHotel.IsActive = hotel.IsActive;

                if (image1 != null)
                {
                    existingHotel.Image1 = await SaveImage(image1);
                }
                if (image2 != null)
                {
                    existingHotel.Image2 = await SaveImage(image2);
                }
                if (image3 != null)
                {
                    existingHotel.Image3 = await SaveImage(image3);
                }


                await _hotelIRepository.UpdateAsync(existingHotel);
                return RedirectToAction(nameof(Index));
            }

            return View(hotel);
        }



        //// GET: KhachSan/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var khachSan = await _context.KhachSan
        //        .Include(k => k.KhuVuc)
        //        .Include(k => k.LoaiKhachSan)
        //        .Include(k => k.NguoiDung)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (khachSan == null)
        //    {
        //        return NotFound();
        //    }
        //    var currentUserId = HttpContext.Session.GetInt32("NguoiDungId");

        //    // Kiểm tra xem người dùng đã đăng nhập chưa
        //    if (!currentUserId.HasValue)
        //    {
        //        // Nếu không có, chuyển hướng đến trang đăng nhập
        //        return RedirectToAction("Login", "NguoiDung");
        //    }

        //    // Kiểm tra xem người dùng có quyền xóa khách sạn này không
        //    if (khachSan.NguoiDungId != currentUserId)
        //    {
        //        // Người dùng không có quyền xóa khách sạn này, chuyển hướng hoặc trả về một thông báo lỗi
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(khachSan);
        //}

        //// POST: KhachSan/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var khachSan = await _context.KhachSan.FindAsync(id);
        //    if (khachSan != null)
        //    {
        //        _context.KhachSan.Remove(khachSan);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool KhachSanExists(int id)
        //{
        //    return _context.KhachSan.Any(e => e.Id == id);
        //}
    }
}
