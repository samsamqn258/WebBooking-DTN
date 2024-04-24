using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;
<<<<<<< HEAD

namespace WebBooking.Data.EF_Repository
{
    public class EvaluateEF_Repository : EvaluateI_Repository
=======
using static WebBooking.Data.I_Repository.EvaluateI_Repository;

namespace WebBooking.Data.EF_Repository
{
    public class EvaluateEF_Repository: EvaluateI_Repository
>>>>>>> 05caec1 (updateDB)
    {
        private readonly MyData _myData;

        public EvaluateEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        public async Task<IEnumerable<Evaluate>> ShowEvaluate(int HotelId)
        {
            var ListEvaluate = await _myData.Evaluates
                .Where(e => e.Payment.Booking.Room.HotelID == HotelId)
<<<<<<< HEAD
                .Include(e => e.Payment) 
                .ThenInclude(p => p.Booking) 
                .ThenInclude(b => b.User) 
=======
                .Include(e => e.Payment)
                .ThenInclude(p => p.Booking)
                .ThenInclude(b => b.User)
>>>>>>> 05caec1 (updateDB)
                .ToListAsync();

            return ListEvaluate;
        }

        public async Task AddAsync(Evaluate evaluate)
        {
            _myData.Evaluates.Add(evaluate);
            await _myData.SaveChangesAsync();
        }
<<<<<<< HEAD
        public async Task <Evaluate> GetByIdAsync(int PaymentId)
=======
        public async Task<Evaluate> GetByIdAsync(int PaymentId)
>>>>>>> 05caec1 (updateDB)
        {
            return _myData.Evaluates.FirstOrDefault(p => p.PaymentID == PaymentId);
        }
    }
}
