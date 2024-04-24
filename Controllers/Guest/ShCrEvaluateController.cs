using Microsoft.AspNetCore.Mvc;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Controllers.Guest
{
    public class ShCrEvaluateController : Controller
    {
        private EvaluateI_Repository _evaluateIRepository;
        public ShCrEvaluateController(EvaluateI_Repository evaluateIRepository)
        {
            _evaluateIRepository = evaluateIRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
