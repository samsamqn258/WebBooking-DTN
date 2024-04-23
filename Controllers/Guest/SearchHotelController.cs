using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class SearchHotelController : Controller
    {
        private HotelI_Repository _hotelIRepository;
        private AreaI_Repository _areaIRepository;
        private HotelTypeI_Repository _hotelTypeIRepository;

        public SearchHotelController(HotelI_Repository hotelRepository, AreaI_Repository areaRepository, HotelTypeI_Repository hotelTypeRepository )
        {
            _hotelIRepository = hotelRepository;
            _areaIRepository = areaRepository;
            _hotelTypeIRepository = hotelTypeRepository;
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
            var availableRooms = await _hotelIRepository.GetAvailableRoomsByHotelIdAsync(hotelId, fromDate, toDate, numberOfGuests);
            if (fromDate == null || toDate == null || fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
            {
                ViewBag.NoDay = "Chọn ngày để xem phòng trống và giá tại chỗ nghỉ này";
             
            }
            if (availableRooms == null || !availableRooms.Any())
            {
                ViewBag.NoAvailableRoomsMessage = "Không có phòng trống trong khoảng thời gian đã chọn.";
            }
          
            ViewBag.ShowResults = availableRooms != null && availableRooms.Any();


            return View(availableRooms);
        }

        public async Task<IActionResult> CheckBoxHotelTypes(int TypeId, DateTime fromDate, DateTime toDate)
        {
            TempData["FromDate"] = fromDate;
            TempData["ToDate"] = toDate;
            var hotelTypes = await _hotelIRepository.ListHoteltype(TypeId);
            return View();
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
        public async Task<IActionResult> AllHotelInArea(int AreaId)
        {
            
            var getNameArea = await _areaIRepository.GetAreaName(AreaId);
            TempData["Area"] = getNameArea.AreaName;
            var ListHotel = await _hotelIRepository.ListHotelInArea(AreaId);
            if (ListHotel == null || !ListHotel.Any())
            {
                ViewBag.NotFoundHotelInArea = "Không có khách sạn nào trong khu vực";
            }
            return View(ListHotel);
        }
        public async Task<IActionResult> AllHotelType(int TypeId)
        {
            var famousHotels = await _hotelIRepository.ListHotelFamous();
            var hotelTypes = await _hotelTypeIRepository.GetAllListHotelType();
            ViewBag.FamousHotels = famousHotels;
            ViewBag.HotelTypes = hotelTypes;
            var getHotelTypeName = await _hotelTypeIRepository.GetHotelTypeName(TypeId);
            TempData["HotelType"] = getHotelTypeName.TypeName;
            var listHotelType = await _hotelIRepository.ListHoteltype(TypeId);
            if (listHotelType == null || !listHotelType.Any())
            {
                ViewBag.NotFoundHotelType = "Không có loại chỗ nghĩ bạn cần tìm";
            }
            return View(listHotelType);
        }
    }
}
