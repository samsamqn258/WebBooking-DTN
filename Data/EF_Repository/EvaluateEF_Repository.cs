using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class EvaluateEF_Repository : EvaluateI_Repository
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
                .ToListAsync();
            return ListEvaluate;
        }
        public async Task AddAsync(Evaluate evaluate)
        {
            _myData.Evaluates.Add(evaluate);
            await _myData.SaveChangesAsync();
        }
    }
}
