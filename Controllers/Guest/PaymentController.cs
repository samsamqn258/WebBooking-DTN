using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Configuration;
using WebBooking.Data.I_Repository;
using Microsoft.Extensions.Configuration;
using WebBooking.Models;
using DPKS_TRUCTUYEN.Controllers;

namespace WebBooking.Controllers.Guest
{
    public class PaymentController : Controller
    {
      
        private BookingI_Repository _bookingIRepository;
        private PaymentI_Repository _paymentIRepository;
        private readonly IConfiguration _configuration;
        public PaymentController( BookingI_Repository bookingRepository, PaymentI_Repository paymentRepository, IConfiguration configuration)
        {

            _bookingIRepository = bookingRepository;
            _paymentIRepository = paymentRepository;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListBill()
        {
            int? guestInfo = HttpContext.Session.GetInt32("UserID");
            if (!guestInfo.HasValue)
            {
                // Handle the case where NguoiDungId is null
                ViewBag.NoGuest = "Khách hàng không tồn tại";
                return View();
            }
            var bill = await _paymentIRepository.GetAllByGuestIdAsync(guestInfo.Value);
            if (bill != null && bill.Any())
            {
                return View(bill);
            }
            else
            {
                ViewBag.NoBill = "Không có hóa đơn nào";
                return View();
            }
           
        }
        public async Task<IActionResult> PaymentMoMo(int bookingId)
        {
            Booking booking = await _bookingIRepository.GetByIdAsyncBooking(bookingId);
            if (booking == null || booking.TotalPriceBooking == 0)
            {
                ViewBag.No = "Phiếu đặt phòng không tồn tại";
            }
            double price = booking.TotalPriceBooking;
            string priceStr = price.ToString(); // Chuyển đổi sang chuỗi mặc định

            // Hoặc bạn có thể chỉ định định dạng số phần thập phân mong muốn
            //string giatienWithFormat = giatien.ToString("N2");

            int newBookingId = booking.BookingID;
            string newBookingIdStr = newBookingId.ToString();

            string endpoint = _configuration.GetValue<string>("MomoAPI:MomoApiUrl");
            string serectkey = _configuration.GetValue<string>("MomoAPI:Serectkey");
            string accessKey = _configuration.GetValue<string>("MomoAPI:AccessKey");
            string returnUrl = _configuration.GetValue<string>("MomoAPI:ReturnUrl");
            string notifyUrl = _configuration.GetValue<string>("MomoAPI:NotifyUrl");
            string partnerCode = _configuration.GetValue<string>("MomoAPI:PartnerCode");
            string orderInfo = newBookingIdStr;
            string amount = priceStr;
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyUrl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
                {
                      { "partnerCode", partnerCode },
                      { "accessKey", accessKey },
                      { "requestId", requestId },
                      { "amount", amount },
                      { "orderId", orderid },
                      { "orderInfo", orderInfo },
                      { "returnUrl", returnUrl },
                      { "notifyUrl", notifyUrl },
                      { "extraData", extraData },
                      { "requestType", "captureMoMoWallet" },
                      { "signature", signature }

                };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        public async Task<IActionResult> ReturnUrl()
        {
            string param = "";
            param += "partnerCode=" + Request.Query["partnerCode"];
            param += "&accessKey=" + Request.Query["accessKey"];
            param += "&requestId=" + Request.Query["requestId"];
            param += "&amount=" + Request.Query["amount"];
            param += "&orderId=" + Request.Query["orderId"];
            param += "&orderInfo=" + Request.Query["orderInfo"];
            param += "&orderType=" + Request.Query["orderType"];
            param += "&transId=" + Request.Query["transId"];
            param += "&message=" + Request.Query["message"];
            param += "&localMessage=" + Request.Query["localMessage"];
            param += "&responseTime=" + Request.Query["responseTime"];
            param += "&errorCode=" + Request.Query["errorCode"];
            param += "&payType=" + Request.Query["payType"];
            param += "&extraData=" + Request.Query["extraData"];

            string newBookingId = Request.Query["orderInfo"];
            int newBookingIdNum = Convert.ToInt32(newBookingId);
            var booking = await _bookingIRepository.GetByIdAsyncBooking(newBookingIdNum);
            //param = Server.UrlEncode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = _configuration.GetValue<string>("MomoAPI:Serectkey");
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request.Query["signature"].ToString())
            {
                ViewBag.message = "Thông tin Request không hợp lệ";
                return View();
            }
            if (!Request.Query["errorCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại";
                return RedirectToAction("Index");
            }
            else
            {
                booking.BookingStatus = true;
                booking.PaymentStatus = true;
                Payment bill = new Payment();
                bill.TypeID = 1;
                bill.PaymentDate = DateTime.Now;
                bill.TotalPayment = booking.TotalPriceBooking;
                bill.BookingID = booking.BookingID;
                await _paymentIRepository.AddAsync(bill);
                ViewBag.message = "Thanh toán thành công";
                await _bookingIRepository.UpdateAsync(booking);
            }

            return View();
        }
    }
}
