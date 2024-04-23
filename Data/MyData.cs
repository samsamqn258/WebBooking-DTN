using Microsoft.EntityFrameworkCore;
using WebBooking.Models;

namespace WebBooking.Data
{
    public class MyData : DbContext
    {
        public MyData(DbContextOptions<MyData> options) : base(options) { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<HotelType> HotelTypes { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<FavoriteHotel> FavoriteHotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<CancelBooking> CancelBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        
            modelBuilder.Entity<Booking>()
                        .HasOne(p => p.User)
                        .WithMany()
                        .HasForeignKey(p => p.UserID)
                        .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<Hotel>()
                .HasOne(k => k.User)
                .WithMany()
                .HasForeignKey(k => k.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteHotel>()
                .HasKey(fh => new { fh.HotelID, fh.UserID });
        }
    }
}
