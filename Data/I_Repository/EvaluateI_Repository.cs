using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
<<<<<<< HEAD
    public interface EvaluateI_Repository
    {
        Task<IEnumerable<Evaluate>> ShowEvaluate(int HotelId);
        Task AddAsync(Evaluate evaluate);
        Task <Evaluate> GetByIdAsync(int PaymentId);

    }
=======

        public interface EvaluateI_Repository
        {
            Task<IEnumerable<Evaluate>> ShowEvaluate(int HotelId);
            Task AddAsync(Evaluate evaluate);
            Task<Evaluate> GetByIdAsync(int PaymentId);

        }
    
>>>>>>> 05caec1 (updateDB)
}
