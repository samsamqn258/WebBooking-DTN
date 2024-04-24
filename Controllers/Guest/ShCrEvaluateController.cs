using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class ShCrEvaluateController : Controller
    {
        private EvaluateI_Repository _evaluateIRepository;
        private HotelI_Repository _hotelIRepository;
        private PaymentI_Repository _paymentIRepository;
        public ShCrEvaluateController(EvaluateI_Repository evaluateIRepository, HotelI_Repository hotelIRepository, PaymentI_Repository paymentIRepository)
        {
            _evaluateIRepository = evaluateIRepository;
            _hotelIRepository = hotelIRepository;
            _paymentIRepository = paymentIRepository;
        }
        public IActionResult Index()
        {
            return View();
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
        public async Task<IActionResult> CreateEvaluate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvaluate(int PaymentID, [Bind("Comment,Score,Image1,Image2")] Evaluate evaluate, IFormFile image1, IFormFile image2)
        {
            var FindBill = await _paymentIRepository.GetByIdAsync(PaymentID);
            if(FindBill == null)
            {
                ModelState.Remove("PaymentID"); 
                ModelState.Remove("evaluate.Comment"); 
                ModelState.Remove("evaluate.Score"); 
                ModelState.Remove("evaluate.Image1"); 
                ModelState.Remove("evaluate.Image2"); 

                ViewBag.NoBill = "Mã hóa đơn không tồn tại";
                return View();

            }
            var FindEvaluate = await _evaluateIRepository.GetByIdAsync(PaymentID);
            if (FindEvaluate != null)
            {
                ViewBag.NoBill = "Bạn đã đánh giá rồi";
                return View();
            }
            if (image1 != null)
            {
                evaluate.Image1 = await SaveImage(image1);
            }
            if (image2 != null)
            {
                evaluate.Image2 = await SaveImage(image2);
            }
            await _evaluateIRepository.AddAsync(evaluate);
            return View();
        }
        public async Task<IActionResult> ShowListEvaluate(int HotelId)
        {
            var ListEvaluate = await _evaluateIRepository.ShowEvaluate(HotelId);
            if (ListEvaluate == null)
            {
                ViewBag.NoHotel = "Khách sạn hiện chưa có đánh giá nào";
            }

            return View(ListEvaluate);
        }
    }
}
