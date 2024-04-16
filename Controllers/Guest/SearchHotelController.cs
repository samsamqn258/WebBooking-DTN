using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;

namespace WebBooking.Controllers.Guest
{
    public class SearchHotelController : Controller
    {
        private HotelI_Repository _hotelIRepository;
     
        public SearchHotelController(HotelI_Repository hotelRepository)
        {
            _hotelIRepository = hotelRepository;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AvailableHotels(DateTime? fromDate, DateTime? toDate, int? numberOfGuests, string location)
        {

            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            TempData["NumberOfGuests"] = numberOfGuests;
            TempData["Area"] = location;

            if (fromDate.HasValue && toDate.HasValue && numberOfGuests.HasValue && !string.IsNullOrEmpty(location))
            {
                // Gọi phương thức GetAvailableHotelsAsync để lấy danh sách khách sạn có sẵn
                var availableHotels = await _hotelIRepository.GetAvailableHotelsAsync(fromDate.Value, toDate.Value, numberOfGuests.Value, location);

                // Kiểm tra null cho danh sách khách sạn trước khi truyền vào view
                if (availableHotels == null || !availableHotels.Any())
                {
                    ViewBag.NoAvailableHotelsMessage = "Không có khách sạn nào trống trong khoảng thời gian đã chọn.";
                }
                ViewBag.ShowResults = availableHotels != null && availableHotels.Any();
                ViewBag.fromDate = fromDate.Value;
                ViewBag.toDate = toDate.Value;
                ViewBag.numberOfGuests = numberOfGuests.Value;

                // Trả về view với danh sách các khách sạn có phòng trống
                return View(availableHotels);
            }
            else
            {
                // Trả về view với thông báo lỗi nếu các tham số không hợp lệ
                ViewBag.NoAvailableHotelsMessage = "Vui lòng nhập đầy đủ thông tin để tìm kiếm khách sạn.";
                ViewBag.ShowResults = false;
                return View();
            }
        }
        public async Task<IActionResult> AvailableRooms(int hotelId, DateTime fromDate, DateTime toDate, int numberOfGuests)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            TempData["NumberOfGuests"] = numberOfGuests;
            TempData["hotelId"] = hotelId;
            // ViewBag.hotelId = hotelId;  
            var availableRooms = await _hotelIRepository.GetAvailableRoomsByHotelIdAsync(hotelId, fromDate, toDate, numberOfGuests);

            if (availableRooms == null || !availableRooms.Any())
            {
                ViewBag.NoAvailableRoomsMessage = "Không có phòng trống trong khoảng thời gian đã chọn.";
            }
            ViewBag.ShowResults = availableRooms != null && availableRooms.Any();


            return View(availableRooms);
        }

        public async Task<IActionResult> AvailableRoomsInHotel(int hotelId, DateTime fromDate, DateTime toDate, int numberOfGuests)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            TempData["NumberOfGuests"] = numberOfGuests;
            TempData["hotelId"] = hotelId;

            var availableRooms = await _hotelIRepository.GetAvailableRoomsByHotelIdAsync(hotelId, fromDate, toDate, numberOfGuests);

            if (availableRooms == null || !availableRooms.Any())
            {
                ViewBag.NoAvailableRoomsMessage = "Không có phòng trống trong khách sạn này trong khoảng thời gian đã chọn.";
            }
            ViewBag.ShowResults = availableRooms != null && availableRooms.Any();

            return View("AvailableRooms", availableRooms);
        }
    }
}
