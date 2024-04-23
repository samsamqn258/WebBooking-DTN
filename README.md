Task Favourite: 
-	Khi nhấn vào nút trái tim hiển thị trạng thái của trái tim sau đó lưu vào danh sách yêu thích của người dùng, trong danh sách yêu thích có thể xóa mục yêu thích, hình ảnh khách sạn, thông tin tên, địa chỉ,…. Trong mục favourite vẫn có nút đặt phòng khách sạn chuyển hướng người dùng đến trang AvailableRoom. Khi không có khách sạn nào trong mục yêu thích thì hiển thị hình ảnh bên dưới (Nút bắt đầu tìm kiếm chuyển hướng người dùng đến trang Index) . ( Phải lấy ra được những danh sách khách sạn yêu thích của đúng người dùng đã thêm vào)
 
Task Envaluate:  
-	Sau khi thanh toán xong sẽ có ID của PĐP, người dùng sẽ dùng ID đấy để vào lại khách sạn mik vừa đặt để đánh giá với điều kiện khách sạn đó phải thuộc phiếu đặt phòng đó và cái phòng đang được đánh giá phải thuộc cái khách sạn đó( có nghĩa là ID PĐP phải là của HotelID và RoomID đó, Userid = booking.Userid,idhotel == bookingid.roomid.hotelid).
-	Sau khi kiểm tra xong nếu mã PĐP đúng thì tiếp tục tiến trình đánh giá là 1 ô input: Nhập mã PĐP, Bên dưới sẽ là nội dung đánh giá. Còn nếu sai thì sẽ xóa sạch nội dung và hiển thị thông báo mã PĐP không hợp lệ.
-	Sau khi đánh giá hoàn tất lưu vào cơ sở dữ liệu sau đó hiển thị nó ra giao diện người dùng bên dưới danh sách phòng.
-	Nội dung của đánh giá bao gồm như ảnh bên dưới: (điểm là sẽ chọn từ thang 1 -> 10, còn tất cả còn lại đều nhập từ người dùng)
-	Kiểm tra xem đó có phải là đánh giá của người dùng hiện tại không nếu có thì hiển thị nút xóa và sửa nội dung còn không thì như mặc định.
-	Sắp xếp nội dung theo điểm từ bé đến lớn và ngược lại 
-	Tính tổng điểm trung bình của khách sạn bằng cách ( cộng tất cả các điểm đánh giá điều kiện phiếu đánh giá đó thuộc khách sạn rồi chia cho tổng lượt đánh giá của khách sạn đó).
Task Room:
-	Xử lý sao cho khi người dùng đang ở trong phòng của khách sạn thì không được ẩn phòng hoặc ẩn khách sạn. 
Task Hotel: 
-      Lấy trong listBookingHotelOnwer, Trạng thái ẩn khách sạn giống giống ẩn phòng.
Task Admin:
-	Duyệt các khách sạn khi đăng tải lên nếu ( đồng ý gán trạng thái duyệt bên bảng hotel bằng true)
-	Quản lí tài khoản của các loại user còn lại
-	Có thể ẩn khách sạn bằng cách làm tưng tự như Task Hotel
-	Hiện số lượng thông báo các khách sạn đang chờ duyệt bằng chuông bên Admin
Task User:
-	Chỉnh sửa ảnh của khách hàng trong mục thông tin của khách hàng như bên hình bên dưới:
 
-	Khi hủy phòng hiện Modal xác nhận lí do hủy phòng sau đó xác nhận hủy ( lí do hủy chọn theo checkbox là 6 lí do)
-      Sửa giao diện của Available Room: là cái khách sạn hiển thị đầu tiên như booking sau đó xuongs chữ      phòng trống cái tìm kiếm rồi cái danh sách phòng.
-       Sửa lại modal khi bấm đặt phòng: hiện thông tin chi tiết như hình ở desktop,
-       Hiển thị modal khi hủy phòng bạn có chắc chắn muốn hủy không
-       Danh Sách Khách Sạn Ưa Thích chức năng xóa
-       Thêm Các điểm đến được chúng tôi ưa thích trong index
 
-       Thêm các nút slide trên index
-       Làm giao diện hiển thị thông báo khi chưa có phòng đặt
-       Mục thay đổi mật khẩu
-       Thêm nút chọn ngôn ngữ trên navbar là  logo  
-  Thêm mục hỗ trợ khách hàng trên navbar  
Task Filter: 
-	Xử lý lọc theo checkbox bên danh sách khách sạn và điểm số khi người dùng checkbox
-	Khi người dùng bỏ check hiển thị lại danh sách như ban đầu.
Task API: 
-	Xử lý google map ( xác định vị trí hiện tại của người dùng đến vị trí của khách sạn)
-	Xử lý dịch văn bản theo yêu cầu của người dùng.
Task Toast:
-	Hiển thị các thông báo bằng toast sử dụng thư viện bên trong Toast, hiển thị với các thông báo sai ở trên và các thông báo thành công của tất cả các thành phần có liên quan đến tính đúng và sai.
Task Booking:
-	Đặt thời gian cố định checkInDate trễ hơn checkOutDate mặc định là ( 14:00 -> 12:00).
Task HotelOwner: 
-	Hiển thị thông báo số lượng trên chuông của hotelOwner là phiếu đặt phòng.
Task Register:
-	Khi nguời dùng đăng kí xong sẽ chuyển đến trang nhập mã xác thực xem thử đó có phải là email thực hay không bằng cách Nếu đúng là email thực thì sau khi đăng kí xong sẽ gửi về cho user 1 đoạn mã xác thực sau khi nhận được user sẽ quay trở lại trang xác thực để nhập mã đó vào nếu đúng thì chuyển hướng sang trang đăng nhập, nếu sai thì báo lỗi bạn đã nhập sai mã xác thực vui lòng kiểm tra lại email xem đã đúng chưa.
-	Xử lý lấy lại mật khẩu khi click vào ô mật khẩu đoạn mật khẩu mới sẽ được gửi về email của người dùng.
Task Enpoint :
-	Làm tới đâu thêm endpoint tới đó
