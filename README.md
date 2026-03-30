# Backend Webtruyen API

Một backend API mạnh mẽ được xây dựng bằng ASP.NET Core 8.0 để quản lý nền tảng đọc truyện tranh trực tuyến (Web Comics Platform). Dự án cung cấp các tính năng hoàn chỉnh từ xác thực người dùng, quản lý truyện, đến thu thập dữ liệu từ các nguồn bên ngoài.

## 🌟 Tính Năng

- **Xác thực & Bảo mật**: JWT token-based authentication
- **Quản lý Truyện**: CRUD operations cho truyện, chương, thể loại
- **Hệ thống Follow**: Người dùng có thể theo dõi các truyện yêu thích
- **Thu thập Dữ liệu**: Module crawl tự động để lấy dữ liệu truyện từ OTruyen API
- **Hỗ trợ Email**: Gửi email cho việc đặt lại mật khẩu và thông báo
- **API Documentation**: Swagger UI để dễ dàng khám phá API
- **CORS Support**: Hỗ trợ kết nối từ các domain khác nhau
- **SQL Server Database**: Sử dụng Entity Framework Core ORM

## 🛠️ Công Nghệ Sử Dụng

- **Framework**: ASP.NET Core 8.0
- **Language**: C#
- **Database**: Microsoft SQL Server (with Entity Framework Core 8.0)
- **Authentication**: JWT Bearer Token
- **API Documentation**: Swagger/OpenAPI
- **Password Hashing**: BCrypt.Net-Next
- **Security**: Google APIs (OAuth support)
- **ORM**: Entity Framework Core

## 📋 Yêu Cầu Trước

- **.NET 8.0 SDK** trở lên
- **SQL Server** 2019 hoặc mới hơn (hoặc SQL Server Express)
- **Visual Studio 2022** hoặc **VS Code** + C# Extension

## 🚀 Cài Đặt

### 1. Clone Repository

```bash
git clone https://github.com/V914p/Backend-Webtruyen-API.git
cd Backend-Webtruyen-API
```

### 2. Khôi Phục Dependencies

```bash
dotnet restore
```

### 3. Cấu Hình Cơ Sở Dữ Liệu

Chỉnh sửa file `appsettings.json` với thông tin kết nối SQL Server của bạn:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=truyen;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS",
    "Issuer": "YourAppName",
    "Audience": "YourAppUsers"
  },
  "Email": {
    "From": "your-email@gmail.com",
    "Password": "your-app-password"
  }
}
```

### 4. Áp Dụng Database Migrations

```bash
dotnet ef database update
```

hoặc tạo migration mới:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## ▶️ Chạy Dự Án

### Development

```bash
dotnet run
```

API sẽ chạy tại `https://localhost:5001` (hoặc `http://localhost:5000` tùy cấu hình)

Swagger UI: `https://localhost:5001/swagger`

### Production

```bash
dotnet publish -c Release
dotnet webtruyenBackEndAPI.dll
```

## 📁 Cấu Trúc Thư Mục

```
├── Controllers/              # API Controllers
│   ├── AuthController.cs    # Xác thực & Đăng ký
│   ├── ComicController.cs   # Quản lý truyện
│   ├── ChaptersController.cs # Quản lý chương
│   ├── GenresController.cs  # Quản lý thể loại
│   ├── FollowController.cs  # Theo dõi truyện
│   └── CrawlController.cs   # Thu thập dữ liệu
├── Models/                   # Entity Models
│   ├── Comic.cs
│   ├── Chapter.cs
│   ├── Genre.cs
│   ├── Account.cs
│   └── ...
├── Data/                     # Database Context & Services
│   ├── AppDbContext.cs      # Entity Framework DbContext
│   ├── AuthService.cs       # Logic xác thực
│   └── EmailService.cs      # Gửi email
├── DTOs/                     # Data Transfer Objects
│   ├── AuthDTOs.cs
│   └── FollowDtos.cs
├── Crawl_comic/              # Module thu thập dữ liệu
│   ├── ComicCrawlerService.cs
│   ├── OTruyenApiClient.cs
│   └── README.md
├── appsettings.json         # Cấu hình mặc định
├── appsettings.Development.json # Cấu hình development
└── Program.cs               # Entry point

```

## 📡 API Endpoints

### Authentication
- `POST /api/auth/register` - Đăng ký tài khoản mới
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/refresh` - Làm mới JWT token
- `POST /api/auth/forgot-password` - Quên mật khẩu
- `POST /api/auth/reset-password` - Đặt lại mật khẩu

### Comics
- `GET /api/comic` - Lấy danh sách truyện
- `GET /api/comic/{id}` - Chi tiết truyện
- `POST /api/comic` - Tạo truyện (Admin)
- `PUT /api/comic/{id}` - Cập nhật truyện (Admin)
- `DELETE /api/comic/{id}` - Xóa truyện (Admin)

### Chapters
- `GET /api/chapters/{comicId}` - Lấy danh sách chương
- `GET /api/chapters/{id}` - Chi tiết chương
- `POST /api/chapters` - Tạo chương (Admin)
- `PUT /api/chapters/{id}` - Cập nhật chương (Admin)
- `DELETE /api/chapters/{id}` - Xóa chương (Admin)

### Genres
- `GET /api/genres` - Danh sách thể loại
- `GET /api/genres/{id}` - Chi tiết thể loại

### Follow
- `GET /api/follow/user/{userId}` - Danh sách truyện đang theo dõi
- `POST /api/follow` - Theo dõi truyện
- `DELETE /api/follow/{comicId}` - Bỏ theo dõi

### Crawl (Admin)
- `POST /api/crawl/latest?page=1` - Thu thập truyện mới từ OTruyen

Xem chi tiết tại Swagger UI: `/swagger`

## 🔄 Module Thu Thập Dữ Liệu (Crawl_comic)

Module này tự động thu thập dữ liệu truyện từ OTruyen API và đồng bộ vào cơ sở dữ liệu.

**Cách sử dụng:**

```bash
POST https://localhost:5001/api/crawl/latest?page=1
```

**Đặc điểm:**
- Lấy danh sách truyện mới từ OTruyen API
- Lấy chi tiết và toàn bộ danh sách chương của từng truyện
- Đồng bộ vào bảng: `comics`, `genres`, `comic_genres`, `chapters`
- Sinh file SQL lưu tất cả các lệnh INSERT/UPDATE ở thư mục `Crawl_comic/output`

Xem thêm: [Crawl_comic/README.md](Crawl_comic/README.md)

## 🔐 Cấu Hình Bảo Mật

### JWT Configuration

Tạo một secret key mạnh (tối thiểu 32 ký tự):

```json
"Jwt": {
  "Key": "YourVeryLongSecretKeyWith32CharactersOrMore",
  "Issuer": "WebTruyenAPI",
  "Audience": "WebTruyenClients"
}
```

### CORS Configuration

Sửa trong `Program.cs` để chỉ cho phép các domain cụ thể:

```csharp
options.AddPolicy("AllowSpecific", policy =>
    policy.WithOrigins("https://yourfrontend.com")
          .AllowAnyHeader()
          .AllowAnyMethod());
```

## 📧 Cấu Hình Email

Để gửi email (reset password, notifications):

```json
"Email": {
  "From": "your-email@gmail.com",
  "Password": "your-app-specific-password"
}
```

Đối với Gmail, bạn cần tạo [App Password](https://support.google.com/accounts/answer/185833).

## 🐛 Troubleshooting

### Lỗi kết nối SQL Server

```
Cannot connect to SQL Server at SERVER_NAME
```

**Giải pháp:**
- Kiểm tra SQL Server có chạy không: `Services.msc` (Windows)
- Verifyication connection string trong `appsettings.json`
- Chắc chắn Windows Authentication hoặc username/password đúng

### Lỗi JWT Token

```
The token has expired or is invalid
```

**Giải pháp:**
- Kiểm tra `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience` trong cấu hình
- Ensure token được gửi đúng format: `Authorization: Bearer YOUR_TOKEN`

### Migration Errors

```
No migrations have been applied to the context
```

**Giải pháp:**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 🤝 Đóng Góp

Hân hạnh chấp nhận Pull Requests! Vui lòng:

1. Fork repository
2. Tạo branch mới: `git checkout -b feature/YourFeature`
3. Commit changes: `git commit -m 'Add YourFeature'`
4. Push to branch: `git push origin feature/YourFeature`
5. Mở Pull Request

## 📝 License

Dự án này được cấp phép dưới [MIT License](LICENSE).

## 📞 Liên Hệ

- **GitHub**: [V914p](https://github.com/V914p)
- **Issues**: [GitHub Issues](https://github.com/V914p/Backend-Webtruyen-API/issues)

---

**Note**: Đây là một dự án đang phát triển. Vui lòng báo cáo bất kỳ lỗi hoặc vấn đề bạn gặp phải qua GitHub Issues.
