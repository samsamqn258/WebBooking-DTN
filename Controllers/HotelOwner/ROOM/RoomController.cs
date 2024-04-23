using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.HotelOwner.ROOM
{
    public class RoomController : Controller
    {
        private RoomI_Repository _roomIRepository;
        private HotelI_Repository _hotelIRepository;
        private RoomTypeI_Repository _roomTypeIRepository;
        public RoomController( RoomI_Repository roomI_Repository, HotelI_Repository hotelI_Repository, RoomTypeI_Repository roomTypeI_Repository)
        {
            _roomIRepository = roomI_Repository;
            _hotelIRepository = hotelI_Repository;
            _roomTypeIRepository = roomTypeI_Repository;
        }
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var hotel = await _hotelIRepository.GetByIdAsync(userId.Value);
            var allRoom = await _roomIRepository.GetAllByAllRoomInHotelIdAsync(hotel.HotelID);

            return View(allRoom);
        }

        public async Task<IActionResult> Details(int roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var room = await _roomIRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        public async Task<IActionResult> Create()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            var hotel = await _hotelIRepository.GetByIdAsync(userId.Value);
            var roomTypes = await _roomTypeIRepository.GetAllListRoomType();
            // Gán ID của khách sạn vào ViewData để sử dụng trong form
            ViewData["HotelId"] = hotel.HotelID;
            // Lấy danh sách loại phòng từ cơ sở dữ liệu và gửi đến view
            ViewData["TypeID"] = new SelectList(roomTypes, "TypeID", "TypeName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("RoomID,TypeID,RoomNumber,PricePerNight,Amenities,Image1,Image2,Image3,IsActive,Discount")] Room room, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            if (ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("UserID");
                var hotel =  await _hotelIRepository.GetByIdAsync(userId.Value);

                room.HotelID = hotel.HotelID;
                // Lưu hình ảnh nếu có
                if (image1 != null)
                {
                    room.Image1 = await SaveImage(image1);
                }
                if (image2 != null)
                {
                    room.Image2 = await SaveImage(image2);
                }
                if (image3 != null)
                {
                    room.Image3 = await SaveImage(image3);
                }

                await _roomIRepository.AddAsync(room);
                return RedirectToAction("Index", "Room", new { hotelId = room.HotelID });
            }

            // Nếu ModelState không hợp lệ, gửi lại form tạo phòng với thông tin đã nhập
            //ViewData["LoaiPhongId"] = new SelectList(_context.LoaiPhong, "Id", "TenLoai", phong.LoaiPhongId);
            return View(room);
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

        public async Task<IActionResult> Edit(int roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var room = await _roomIRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            var roomTypes = await _roomTypeIRepository.GetAllListRoomType();
            ViewData["TypeID"] = new SelectList(roomTypes, "TypeID", "TypeName", room.TypeID);
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int roomId, [Bind("RoomID,TypeID,RoomNumber,PricePerNight,Amenities,Image1,Image2,Image3,IsActive,Discount")] Room room, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            ModelState.Remove("Image1");
            ModelState.Remove("Image2");
            ModelState.Remove("Image3");

            if (roomId != room.RoomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    var originalRoom = await _roomIRepository.GetByIdAsync(roomId);
                    if (originalRoom == null)
                    {
                        return NotFound();
                    }


                    // Kiểm tra và cập nhật ảnh nếu có
                    if (image1 != null)
                    {
                        originalRoom.Image1 = await SaveImage(image1);
                    }
                    if (image2 != null)
                    {
                        originalRoom.Image2 = await SaveImage(image2);
                    }
                    if (image3 != null)
                    {
                        originalRoom.Image3 = await SaveImage(image3);
                    }

                    // Cập nhật các thuộc tính khác của phong từ dữ liệu đầu vào
                    originalRoom.TypeID = room.TypeID;
                    originalRoom.RoomNumber = room.RoomNumber;
                    originalRoom.PricePerNight = room.PricePerNight;
                    originalRoom.Amenities = room.Amenities;
                    originalRoom.IsActive = room.IsActive;
                    originalRoom.Discount = room.Discount;

                    await _roomIRepository.UpdateAsync(originalRoom);
                
               
                // Chuyển hướng người dùng về trang RoomOfKhachSan
                return RedirectToAction("Index", "Room");
            }
            // Nếu ModelState không hợp lệ, trả về lại View với dữ liệu và thông báo lỗi
            var roomTypes = await _roomTypeIRepository.GetAllListRoomType();
            ViewData["TypeID"] = new SelectList(roomTypes, "TypeID", "TypeName", room.TypeID);
            return View(room);
        }

        public async Task<IActionResult> HiddenRoom(int roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var room = await _roomIRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            else
            {
                room.IsActive = false;
                await _roomIRepository.UpdateAsync(room);
              
            }
            return RedirectToAction("Index", "Room", new { hotelId = room.HotelID });
        }

        public async Task<IActionResult> UnHiddenRoom(int roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            var room = await _roomIRepository.GetByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            else
            {
                room.IsActive = true;
                await _roomIRepository.UpdateAsync(room);

            }
            return RedirectToAction("ListRoomHidden", "Room");
        }

        public async Task<IActionResult> ListRoomHidden(int hotelId)

        {
            var listRoomHidden =  await _roomIRepository.GetAllByAllRoomInHotelIdHiddenAsync(hotelId);

            return View(listRoomHidden);
        }

    }
}
