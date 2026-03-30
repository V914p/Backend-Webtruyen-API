# Crawl_comic module

Module này cung cấp API `POST /api/crawl/latest?page=1` để crawl dữ liệu truyện từ OTruyen API.

## Chức năng chính

- Gọi API danh sách truyện mới (`/v1/api/danh-sach/truyen-moi`).
- Lấy chi tiết từng truyện, bao gồm toàn bộ danh sách chương.
- Đồng bộ dữ liệu vào SQL Server theo các bảng `comics`, `genres`, `comic_genres`, `chapters`.
- Sinh file `.sql` ở thư mục `Crawl_comic/output` chứa toàn bộ câu lệnh INSERT/UPDATE tương ứng với thao tác crawl.

## Cách sử dụng

1. Khởi chạy backend (`dotnet run`).
2. Gửi yêu cầu POST tới `https://localhost:5001/api/crawl/latest?page=1` (tuỳ cấu hình `launchSettings.json`).
3. Phản hồi sẽ trả về thống kê crawl và đường dẫn file `.sql` đã sinh.

> Lưu ý: API mặc định crawl trang 1. Có thể thay đổi số trang thông qua query `page`.
