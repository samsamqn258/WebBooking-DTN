using WebBooking.Models;

namespace WebBooking.Data.I_Repository
{
    public interface FavoriteHotelI_Repository
    {
        Task<IEnumerable<FavoriteHotel>> GetAllByAllRoomInHotelIdAsync(int GuestID);
        Task AddAsync(FavoriteHotel favoriteHotel);
        Task UpdateAsync(FavoriteHotel favoriteHotel);
        Task DeleteAsync(int FavoriteHotelID);
    }
}
