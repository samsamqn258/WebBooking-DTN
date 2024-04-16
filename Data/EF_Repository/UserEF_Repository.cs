using Microsoft.EntityFrameworkCore;
using WebBooking.Data.I_Repository;
using WebBooking.Models;

namespace WebBooking.Data.EF_Repository
{
    public class UserEF_Repository : UserI_Repository
    {
        private readonly MyData _myData;

        public UserEF_Repository(MyData myData)
        {
            _myData = myData;
        }
        // Task<NguoiDung> GetByIdAsync(int id);
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _myData.Users.FirstOrDefaultAsync(n => n.UserID == userId);
        }

        public async Task<User> CheckEmail(string email) {

            return await _myData.Users.FirstOrDefaultAsync(n => n.Email == email);
        }
           
        public async Task AddAsync(User user)
        {
            _myData.Users.Add(user);
            await _myData.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            _myData.Users.Update(user);
            await _myData.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _myData.Users.ToListAsync();
        }
    }
}
