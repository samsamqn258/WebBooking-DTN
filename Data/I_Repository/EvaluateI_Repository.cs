using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{

        public interface EvaluateI_Repository
        {
            Task<IEnumerable<Evaluate>> ShowEvaluate(int HotelId);
            Task AddAsync(Evaluate evaluate);
            Task<Evaluate> GetByIdAsync(int PaymentId);

        }
    
}
