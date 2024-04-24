using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using WebBooking.Data;
using WebBooking.Data.EF_Repository;
using WebBooking.Data.I_Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyData>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
builder.Services.AddScoped<UserI_Repository, UserEF_Repository>();
builder.Services.AddScoped<HotelI_Repository, HotelEF_Repository>();
builder.Services.AddScoped<BookingI_Repository, BookingEF_Repository>();
builder.Services.AddScoped<RoomI_Repository, RoomEF_Repository>();
builder.Services.AddScoped<CancelBookingI_Repository, CancelBookingEF_Repository>();
builder.Services.AddScoped<PaymentI_Repository, PaymentEF_Repository>();
builder.Services.AddScoped<AreaI_Repository, AreaEF_Repository>();
builder.Services.AddScoped<HotelTypeI_Repository, HotelTypeEF_Repository>();
builder.Services.AddScoped<RoomTypeI_Repository, RoomTypeEF_Repository>();
builder.Services.AddScoped<EvaluateI_Repository, EvaluateEF_Repository>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
AddCookie(options =>
{
    options.Cookie.Name = "GoodBookingCookie";
    options.LoginPath = "/User/Login";
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//}).AddGoogle(GoogleDefaults.AuthenticationScheme, option =>
//{
//    option.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
//    option.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
//});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".WebBooking.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
           name: "register-guest",
           pattern: "register-guest",
           defaults: new { controller = "AccountGuest", action = "RegisterGuest" });

    endpoints.MapControllerRoute(
           name: "login-guest",
           pattern: "login-guest",
           defaults: new { controller = "AccountGuest", action = "LoginGuest" });

    endpoints.MapControllerRoute(
        name: "logout-guest",
        pattern: "logout-guest",
        defaults: new { controller = "AccountGuest", action = "LogOutGuest" });

    endpoints.MapControllerRoute(
         name: "personal-information",
         pattern: "personal-information",
         defaults: new { controller = "Guest", action = "DisplayInfoGuest" });

    endpoints.MapControllerRoute(
        name: "mytrips",
        pattern: "mytrips",
        defaults: new { controller = "CreateBooking", action = "ListYourBookingForm" });

    endpoints.MapControllerRoute(
        name: "cancel-mytrips",
        pattern: "cancel-mytrips",
        defaults: new { controller = "CancelBooking", action = "ListYourCancelBookingForm" });

    endpoints.MapControllerRoute(
        name: "mybills",
        pattern: "mybills",
        defaults: new { controller = "Payment", action = "ListBill" });
    endpoints.MapControllerRoute(
        name: "home",
        pattern: "home",
        defaults: new { controller = "Guest", action = "Index" });
  

    // =================================== HOTELOWNER ================================
    endpoints.MapControllerRoute(
       name: "logout-user",
       pattern: "logout-user",
       defaults: new { controller = "AccountHotelOwner", action = "LogOutHotelOwner" });
    endpoints.MapControllerRoute(
      name: "hotel-registration",
      pattern: "hotel-registration",
      defaults: new { controller = "AccountHotelOwner", action = "LogOutHotelOwner" });

    //endpoints.MapControllerRoute(
    //   name: "dang-ky-nguoi-dung",
    //   pattern: "dang-ky-nguoi-dung",
    //   defaults: new { controller = "NguoiDung", action = "Register" });

    //endpoints.MapControllerRoute(
    //   name: "dang-nhap-nguoi-dung",
    //   pattern: "dang-nhap-nguoi-dung",
    //   defaults: new { controller = "NguoiDung", action = "Login" });



    //endpoints.MapControllerRoute(
    // name: "dang-xuat-khachhang",
    // pattern: "dang-xuat-khachhang",
    // defaults: new { controller = "KhachHang", action = "DeleteCookie" });


    //endpoints.MapControllerRoute(
    //   name: "dang-xuat-nguoidung",
    //   pattern: "dang-xuat-nguoidung",
    //   defaults: new { controller = "NguoiDung", action = "LogOutND" });

    //endpoints.MapControllerRoute(
    //   name: "trang-chu-nguoi-dung",
    //   pattern: "trang-chu-nguoi-dung",
    //   defaults: new { controller = "NguoiDung", action = "Index" });

    // endpoints.MapControllerRoute(
    //name: "update-phieu-dat-phong",
    //pattern: "update-phieu-dat-phong",
    //defaults: new { controller = "NguoiDung", action = "UpdatePhieuDatPhong" });

    // endpoints.MapControllerRoute(
    //name: "luu-info-nguoi-dung",
    //pattern: "luu-info-nguoi-dung",
    //defaults: new { controller = "KhachHang", action = "SaveCustomerInfo" });

    // endpoints.MapControllerRoute(
    //name: "thanh-toan-tien-mat",
    //pattern: "thanh-toan-tien-mat",
    //defaults: new { controller = "NguoiDung", action = "ThanhToanTienMat" });

    //    endpoints.MapControllerRoute(
    //name: "tao-phong",
    //pattern: "tao-phong",
    //defaults: new { controller = "Phong", action = "Create" });



    //    endpoints.MapControllerRoute(
    //name: "phong-trong",
    //pattern: "phong-trong",
    //defaults: new { controller = "SearchHotel", action = "AvailableRooms" });

    //    endpoints.MapControllerRoute(
    //name: "thanh-toan-momo",
    //pattern: "thanh-toan-momo",
    //defaults: new { controller = "Payment", action = "PaymentMoMo" });

    //    endpoints.MapControllerRoute(
    //   name: "huy-dat-phong",
    //   pattern: "huy-dat-phong",
    //   defaults: new { controller = "KhachHang", action = "CancelBooking" });

    //    endpoints.MapControllerRoute(
    //name: "loai-khach-san",
    //pattern: "loai-khach-san",
    //defaults: new { controller = "LoaiKhachSan", action = "GetHotelsByType" });

    //    endpoints.MapControllerRoute(
    //name: "loc-theo-thanh-pho",
    //pattern: "loc-theo-thanh-pho",
    //defaults: new { controller = "LocCity", action = "LocTheoCity" });

    //    endpoints.MapControllerRoute(
    //name: "tao-phieu-dat-phong",
    //pattern: "tao-phieu-dat-phong",
    //defaults: new { controller = "KhachHang", action = "TaoPhieuDatPhong" });

    //    endpoints.MapControllerRoute(
    //name: "filter",
    //pattern: "filter",
    //defaults: new { controller = "LocLoaiKhachSan", action = "LocRadio" });

    //    endpoints.MapControllerRoute(
    //name: "romm-of-khach-san",
    //pattern: "room-of-khach-san",
    //defaults: new { controller = "Phong", action = "RoomOfKhachSan" });

    //    endpoints.MapControllerRoute(
    //name: "danh-sach-phieu-dat-phong",
    //pattern: "danh-sach-phieu-dat-phong",
    //defaults: new { controller = "NguoiDung", action = "DanhSachPhieuDatKS" });

    //    endpoints.MapControllerRoute(
    //name: "danh-sach-phieu-thanh-toan",
    //pattern: "danh-sach-phieu-thanh-toan",
    //defaults: new { controller = "NguoiDung", action = "DanhSachPhieuThanhToan" });

    //    endpoints.MapControllerRoute(
    //name: "danh-sach-phieu-dat-xac-nhan",
    //pattern: "danh-sach-phieu-dat-xac-nhan",
    //defaults: new { controller = "NguoiDung", action = "DanhSachPhieuDatDaXacNhan" });

    //    endpoints.MapControllerRoute(
    //name: "an-phong",
    //pattern: "an-phong",
    //defaults: new { controller = "Phong", action = "RoomHiden" });



    //    endpoints.MapControllerRoute(
    //    name: "thong-tin-nguoi-dung",
    //    pattern: "thong-tin-nguoi-dung",
    //    defaults: new { controller = "NguoiDung", action = "DisplayInfoND" });

});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Guest}/{action=Index}/{id?}");

app.Run();
