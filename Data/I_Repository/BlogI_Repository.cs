using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface BlogI_Repository
    {
        Task AddAsync(Blog blog);
        Task UpdateAsync(Blog blog);
    }
}
