using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface UserI_Repository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User  user);
        Task UpdateAsync(User user);

        Task<User> CheckEmail(string email);
    }
}
