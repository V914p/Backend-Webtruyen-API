# 📚 WebTruyen API - Backend (Java Spring Boot)

A complete migration of **WebTruyen Backend API** from **ASP.NET Core** to **Java Spring Boot 3.2**, featuring a comprehensive comic management system with real-time notifications, JWT authentication, and advanced crawling capabilities.

**Status**: ✅ Production Ready | **Java Version**: 17+ | **Spring Boot**: 3.2.0

---

## 🚀 Features

✅ **Authentication & Authorization**
- JWT token-based authentication
- Role-based access control (User, Admin)
- Password reset functionality with email verification
- Google OAuth integration support

✅ **Comic Management**
- Full CRUD operations for comics and chapters
- Genre categorization system
- Comic search and pagination
- Comic status tracking (ongoing, completed, dropped)
- Advanced filtering by genre and status

✅ **User Features**
- User profile management
- Follow/unfollow comics and users
- Favorite comics list
- User activity tracking

✅ **Real-time Notifications**
- WebSocket-based notifications using STOMP protocol
- Broadcast notifications to all users
- User-specific notifications
- Notification persistence and history

✅ **Comic Crawling**
- Automatic crawling from external APIs (OTruyen)
- Multi-page crawling support
- Dynamic chapter fetching
- Scheduled crawling tasks

✅ **Admin Dashboard**
- User management
- Comic moderation
- Notification management
- System statistics and analytics

✅ **API Documentation**
- Auto-generated Swagger/OpenAPI documentation
- Interactive API testing interface
- Complete endpoint documentation

---

## 🛠 Tech Stack

| Category | Technology |
|----------|------------|
| **Language** | Java 17+ |
| **Framework** | Spring Boot 3.2.0 |
| **ORM** | Spring Data JPA / Hibernate |
| **Security** | Spring Security + JWT (JJWT) |
| **Real-time** | Spring WebSocket + STOMP |
| **Database** | SQL Server (MSSQL) |
| **Email** | Spring Mail + JavaMail |
| **API Docs** | Springdoc OpenAPI + Swagger UI |
| **Build Tool** | Maven 3.6+ |
| **Data Transfer** | Jackson JSON |

---

## 📋 Tech Stack - C# to Java Migration Map

| C# .NET | Java Spring Boot |
|---------|-----------------|
| ASP.NET Core | Spring Boot |
| Entity Framework Core | Spring Data JPA / Hibernate |
| DbContext | JpaRepository |
| BCrypt.Net | spring-security-crypto |
| JWT Bearer Token | JJWT (io.jsonwebtoken) |
| SignalR | Spring WebSocket + STOMP |
| SMTP (System.Net.Mail) | Spring Mail + JavaMail |
| HttpClient | RestTemplate / WebClient |
| Swashbuckle.AspNetCore | springdoc-openapi-starter-webmvc-ui |
| CORS Policy | @CrossOrigin / SecurityFilterChain |
| AutoMapper | ModelMapper / MapStruct |
| Dependency Injection | Spring Context (IoC) |


## 📁 Project Structure

```
webtruyenBackEndAPI-SpringBoot/
│
├── 📄 pom.xml                    # Maven dependencies & build configuration
├── 📄 README.md                  # This file - complete documentation
├── 📄 QUICK_START.md             # Getting started in 5 minutes
├── 📄 MIGRATION_GUIDE.md         # Detailed C# → Java migration mapping
├── 📄 ARCHITECTURE.md            # System architecture & design patterns
├── 📄 LICENSE                    # Project license
│
├── 📁 src/main/java/com/webtruyenapi/
│   │
│   ├── 📄 WebTruyenApiApplication.java  # Spring Boot main class
│   │
│   ├── 📁 config/                       # Configuration classes
│   │   ├── AppConfig.java               # Spring Bean configurations
│   │   ├── SecurityConfig.java          # JWT & security configuration
│   │   ├── JwtTokenProvider.java        # Token generation & validation
│   │   ├── JwtAuthenticationFilter.java # JWT request filter
│   │   ├── WebSocketConfig.java         # WebSocket & STOMP setup
│   │   ├── OpenApiConfig.java           # OpenAPI grouping config
│   │   └── SwaggerConfig.java           # Swagger UI customization
│   │
│   ├── 📁 controller/                   # REST API endpoints
│   │   ├── AuthController.java          # Authentication (login, register, reset)
│   │   ├── ComicController.java         # Comic CRUD & search
│   │   ├── ChapterController.java       # Chapter CRUD operations
│   │   ├── GenreController.java         # Genre management
│   │   ├── FollowController.java        # Follow/unfollow features
│   │   ├── CommentController.java       # Comments on comics
│   │   ├── NotificationController.java  # Notification endpoints
│   │   ├── CrawlController.java         # Comic crawling
│   │   └── 📁 admin/                    # Admin-specific endpoints
│   │       ├── AdminComicController.java
│   │       ├── AdminUserController.java
│   │       ├── AdminNotificationController.java
│   │       └── AdminDashboardController.java
│   │
│   ├── 📁 service/                      # Business logic layer
│   │   ├── AuthService.java             # Authentication service
│   │   ├── ComicFollowService.java      # Follow/subscription logic
│   │   ├── CommentService.java          # Comment operations
│   │   ├── EmailService.java            # Email sending
│   │   ├── ComicCrawlerService.java     # Comic crawling logic
│   │   ├── OTruyenApiClient.java        # External API client
│   │   ├── GoogleAuthService.java       # Google OAuth integration
│   │   └── 📁 admin/                    # Admin services
│   │       ├── AdminComicService.java
│   │       ├── AdminUserService.java
│   │       ├── AdminDashboardService.java
│   │       └── NotificationService.java
│   │
│   ├── 📁 entity/                       # JPA Entity classes
│   │   ├── Account.java                 # User account entity
│   │   ├── AccountStatus.java           # Account status enum
│   │   ├── Comic.java                   # Comic entity
│   │   ├── ComicStatus.java             # Comic status enum
│   │   ├── ComicGenre.java              # Comic-Genre relationship
│   │   ├── Chapter.java                 # Chapter entity
│   │   ├── Genre.java                   # Genre entity
│   │   ├── Follow.java                  # Follow entity
│   │   ├── ComicFollow.java             # Comic follow entity
│   │   ├── Comment.java                 # Comment entity
│   │   ├── Role.java                    # Role entity
│   │   ├── Notification.java            # Notification entity
│   │   └── PasswordResetToken.java      # Password reset token entity
│   │
│   ├── 📁 repository/                   # Data access layer (JPA Repositories)
│   │   ├── AccountRepository.java
│   │   ├── ComicRepository.java
│   │   ├── ChapterRepository.java
│   │   ├── GenreRepository.java
│   │   ├── ComicGenreRepository.java
│   │   ├── FollowRepository.java
│   │   ├── ComicFollowRepository.java
│   │   ├── CommentRepository.java
│   │   ├── NotificationRepository.java
│   │   └── PasswordResetTokenRepository.java
│   │
│   ├── 📁 dto/                          # Data Transfer Objects
│   │   ├── AuthDTOs.java                # Login/Register/Reset DTOs
│   │   ├── ChapterDTO.java              # Chapter DTO
│   │   ├── ComicDetailDTO.java          # Detailed comic DTO
│   │   ├── ComicSummaryDTO.java         # Summary comic DTO
│   │   ├── CommentRequest.java          # Comment request DTO
│   │   ├── FollowComicDto.java          # Follow/unfollow request
│   │   ├── FollowDtos.java              # Follow-related DTOs
│   │   ├── OTruyenDtos.java             # External API response DTOs
│   │   └── 📁 admin/                    # Admin DTOs
│   │       ├── UserManagementDTO.java
│   │       ├── ComicManagementDTO.java
│   │       ├── DashboardStatsDTO.java
│   │       ├── NotificationRequest.java
│   │       └── BroadcastNotificationRequest.java
│   │
│   └── 📁 websocket/                    # WebSocket handlers
│       └── NotificationHandler.java     # Real-time notification handler
│
└── 📁 src/main/resources/
    └── 📄 application.yml               # Application configuration
```

---

## 📋 Prerequisites & Installation

### Requirements
- **Java 17 or higher** - [Download JDK](https://adoptopenjdk.net/)
- **Maven 3.6+** - [Download Maven](https://maven.apache.org/download.cgi)
- **SQL Server** - Local or remote instance
- **Git** - For version control

### Step 1: Clone the Repository

```bash
# Using HTTPS
git clone -b Java https://github.com/V914p/Backend-Webtruyen-API.git
cd Backend-Webtruyen-API

# Using SSH (if configured)
git clone -b Java git@github.com:V914p/Backend-Webtruyen-API.git
cd Backend-Webtruyen-API
```

### Step 2: Configure Database Connection

Edit `src/main/resources/application.yml`:

```yaml
spring:
  datasource:
    url: jdbc:sqlserver://YOUR_SERVER:1433;databaseName=truyen;trustServerCertificate=true;encrypt=true
    username: sa              # Your SQL Server username
    password: YOUR_PASSWORD   # Your SQL Server password
    driver-class-name: com.microsoft.sqlserver.jdbc.SQLServerDriver
  
  jpa:
    hibernate:
      ddl-auto: update        # create-drop | update | validate | none
    properties:
      hibernate:
        dialect: org.hibernate.dialect.SQLServer2012Dialect
  
  # Email Configuration (for password reset, notifications)
  mail:
    host: smtp.gmail.com
    port: 587
    username: your-email@gmail.com
    password: your-app-password  # Use App Password for Gmail
    properties:
      mail:
        smtp:
          auth: true
          starttls.enable: true
          starttls.required: true

# JWT Configuration
app:
  jwtSecret: your-secret-key-min-32-chars-for-production
  jwtExpiration: 86400000  # 24 hours in milliseconds
```

### Step 3: Build the Project

```bash
# Clean and build
mvn clean install

# Skip tests if needed (faster)
mvn clean install -DskipTests
```

### Step 4: Run the Application

```bash
# Using Maven
mvn spring-boot:run

# Or build a JAR and run
mvn clean package
java -jar target/webtruyenapi-1.0.0.jar

# With custom port
java -jar target/webtruyenapi-1.0.0.jar --server.port=8080
```

✅ **API Running at**: http://localhost:8080/api  
📚 **Swagger UI**: http://localhost:8080/api/swagger-ui.html  
📖 **API Docs**: http://localhost:8080/api/v3/api-docs

---

## 🔌 API Endpoints Overview

### Authentication Endpoints
```
POST   /api/auth/register              - Register new account
POST   /api/auth/login                 - Login & get JWT token
POST   /api/auth/request-reset         - Request password reset
POST   /api/auth/reset-password        - Reset password with token
GET    /api/auth/me                    - Get current user info (requires token)
POST   /api/auth/refresh               - Refresh JWT token
POST   /api/auth/logout                - Logout (optional)
```

### Comic Endpoints
```
GET    /api/comics/page?page=0&size=20  - Paginated comics list
GET    /api/comics/search?q=keyword     - Search comics
GET    /api/comics/{comicId}            - Get comic details
GET    /api/comics/genre/{genreId}      - Comics by genre
GET    /api/comics/status/{status}      - Comics by status
POST   /api/comics                      - Create comic (admin)
PUT    /api/comics/{comicId}            - Update comic (admin)
DELETE /api/comics/{comicId}            - Delete comic (admin)
```

### Chapter Endpoints
```
GET    /api/chapters/comic/{comicId}    - Get all chapters of comic
GET    /api/chapters/{chapterId}        - Get chapter details
POST   /api/chapters                    - Create chapter (admin)
PUT    /api/chapters/{chapterId}        - Update chapter (admin)
DELETE /api/chapters/{chapterId}        - Delete chapter (admin)
```

### Genre Endpoints
```
GET    /api/genres                      - Get all genres
GET    /api/genres/{genreId}            - Get genre details
POST   /api/genres                      - Create genre (admin)
PUT    /api/genres/{genreId}            - Update genre (admin)
DELETE /api/genres/{genreId}            - Delete genre (admin)
```

### Follow Endpoints
```
POST   /api/follows/comic               - Follow a comic (requires token)
DELETE /api/follows/comic/{comicId}     - Unfollow a comic (requires token)
GET    /api/follows/comics              - Get user's followed comics (requires token)
GET    /api/follows/comics/page         - Paginated followed comics
POST   /api/follows/user                - Follow a user (requires token)
DELETE /api/follows/user/{userId}       - Unfollow a user (requires token)
```

### Comment Endpoints
```
GET    /api/comments/comic/{comicId}    - Get comments for comic
POST   /api/comments                    - Create comment (requires token)
PUT    /api/comments/{commentId}        - Update comment (own comment)
DELETE /api/comments/{commentId}        - Delete comment (own or admin)
```

### Notification Endpoints
```
GET    /api/notifications               - Get user notifications (requires token)
POST   /api/notifications/send          - Broadcast notification (admin)
POST   /api/notifications/send-to-user  - Send to specific user (admin)
WS     /api/ws/notifications            - WebSocket STOMP connection
```

### Admin Endpoints
```
GET    /api/admin/dashboard/stats       - Dashboard statistics (admin)
GET    /api/admin/comics/page           - Manage comics list (admin)
PUT    /api/admin/comics/{id}/status    - Update comic status (admin)
GET    /api/admin/users/page            - Manage users list (admin)
PUT    /api/admin/users/{id}/status     - Update user status (admin)
POST   /api/admin/notifications/send    - Send notifications (admin)
```

### Crawling Endpoints
```
POST   /api/crawl/latest?page=1         - Crawl latest comics from source
```

---

## 🔐 Authentication & JWT

### Getting a Token

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "loginName": "user@example.com",
    "password": "password123"
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "accountId": "550e8400-e29b-41d4-a716-446655440000",
    "userName": "john_doe",
    "mail": "user@example.com",
    "role": "USER"
  }
}
```

### Using the Token

All protected endpoints require the JWT token in the Authorization header:

```bash
curl -X GET http://localhost:8080/api/auth/me \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Token Claims
- `sub` - User ID (UUID)
- `email` - User email
- `role` - User role (USER, ADMIN)
- `iat` - Issued at
- `exp` - Expiration time (24 hours)

---

## 🔄 WebSocket Real-time Notifications

### Connect to WebSocket

```javascript
// Client-side example
const socket = new SockJS('http://localhost:8080/api/ws/notifications');
const stompClient = Stomp.over(socket);

stompClient.connect({}, (frame) => {
  console.log('Connected:', frame);
  
  // Subscribe to personal notifications
  stompClient.subscribe('/user/queue/notifications', (message) => {
    console.log('Notification received:', JSON.parse(message.body));
  });
  
  // Subscribe to broadcast notifications
  stompClient.subscribe('/topic/notifications', (message) => {
    console.log('Broadcast:', JSON.parse(message.body));
  });
});
```

### Send Notification (Admin)

```bash
curl -X POST http://localhost:8080/api/notifications/send \
  -H "Authorization: Bearer TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "New Episode Released!",
    "message": "Chapter 50 is now available",
    "type": "NEW_CHAPTER"
  }'
```

---

## 🐛 Debugging & Logging

Enable detailed logging by adding to `application.yml`:

```yaml
logging:
  level:
    root: INFO
    com.webtruyenapi: DEBUG
    org.springframework.security: DEBUG
    org.hibernate.SQL: DEBUG
    org.hibernate.type.descriptor.sql.BasicBinder: TRACE
```

---

## 📦 Database Schema

The application automatically creates tables on first run. Main tables:

- **account** - User accounts
- **comic** - Comic information
- **chapter** - Comic chapters
- **genre** - Comic genres
- **comic_genre** - Comic-Genre relationship (many-to-many)
- **follow** - User follows user
- **comic_follow** - User follows comic
- **comment** - Comments on comics
- **notification** - Notification records
- **password_reset_token** - Password reset tokens
- **role** - User roles

### Docker Deployment

Create a `Dockerfile` in the project root:

```dockerfile
FROM openjdk:17-jdk-slim

WORKDIR /app

COPY target/webtruyenapi-1.0.0.jar app.jar

EXPOSE 8080

ENTRYPOINT ["java", "-jar", "app.jar"]
```

Build and run:

```bash
# Build
mvn clean package -DskipTests
docker build -t webtruyenapi:latest .

# Run
docker run -p 8080:8080 \
  -e SPRING_DATASOURCE_URL="jdbc:sqlserver://host.docker.internal:1433;databaseName=truyen" \
  -e SPRING_DATASOURCE_USERNAME="sa" \
  -e SPRING_DATASOURCE_PASSWORD="YOUR_PASSWORD" \
  webtruyenapi:latest
```

### Production Deployment Checklist

- [ ] Set `spring.jpa.hibernate.ddl-auto: validate` in production
- [ ] Use environment variables for sensitive data (database credentials, JWT secret)
- [ ] Configure HTTPS/SSL certificates
- [ ] Set up database backups
- [ ] Configure logging to files (not just console)
- [ ] Set up monitoring and alerting
- [ ] Enable CORS for frontend domain only
- [ ] Use strong JWT secret (min 32 characters)
- [ ] Configure rate limiting on API endpoints
- [ ] Set up automated database migrations
- [ ] Use connection pooling (HikariCP - built-in with Spring Boot)

---

## 🔧 Common Issues & Troubleshooting

### Issue: "Connection refused" to SQL Server

**Solution**:
```yaml
# Make sure trustServerCertificate is set
spring:
  datasource:
    url: jdbc:sqlserver://localhost:1433;databaseName=truyen;trustServerCertificate=true
```

### Issue: "JWT token expired"

Check application.yml:
```yaml
app:
  jwtExpiration: 86400000  # 24 hours (adjust as needed)
```

### Issue: WebSocket connection fails

Ensure WebSocket is properly configured:
```yaml
spring:
  websocket:
    enabled: true
```

### Issue: Email not working

Gmail / Office 365:
```yaml
spring:
  mail:
    host: smtp.gmail.com
    port: 587
    properties:
      mail:
        smtp:
          auth: true
          starttls.enable: true
          starttls.required: true
```

Check that you're using **App Passwords** (not regular password) for Gmail.

### Issue: Build fails with "Cannot find com.microsoft:mssql-jdbc"

Update Maven and rebuild:
```bash
mvn clean install -U
```

---

## 📚 Additional Resources

- [Spring Boot Documentation](https://spring.io/projects/spring-boot)
- [Spring Security with JWT](https://spring.io/projects/spring-security)
- [Spring Data JPA](https://spring.io/projects/spring-data-jpa)
- [SQL Server JDBC Driver](https://learn.microsoft.com/en-us/sql/connect/jdbc/download-microsoft-jdbc-driver-for-sql-server)
- [JWT.io - JWT Introduction](https://jwt.io/introduction)
- [Swagger/OpenAPI Documentation](https://swagger.io/)

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## ✨ Key Improvements from C# Version

✅ Better performance with Direct SQL Server connection pooling
✅ More modular architecture with separate service layers
✅ Built-in Swagger UI for API documentation
✅ Simplified WebSocket implementation with Spring WebSocket
✅ Easier deployment with Docker support
✅ Better scalability with Spring framework ecosystem
✅ Comprehensive error handling and validation
✅ Role-based access control (RBAC) with Spring Security
✅ Automatic database schema generation
✅ Built-in health checks and metrics (Spring Boot Actuator)

---

## 📞 Support

For issues, questions, or suggestions:
- Open an issue on GitHub
- Check existing documentation in QUICK_START.md, MIGRATION_GUIDE.md, ARCHITECTURE.md
- Review Swagger UI at `/api/swagger-ui.html`

---

**Last Updated**: 2026-03-30  
**Version**: 1.0.0  
**Status**: Production Ready ✅
       username: your-email@gmail.com
       password: your-app-password  # Use Gmail App Password
   ```

4. **Build project**:
   ```bash
   mvn clean install
   ```

5. **Run application**:
   ```bash
   mvn spring-boot:run
   ```

   Hoặc jar file:
   ```bash
   java -jar target/webtruyenapi-1.0.0.jar
   ```

- API sẽ chạy tại: **http://localhost:8080/api**
- Swagger UI: **http://localhost:8080/api/swagger-ui.html**
- WebSocket: **ws://localhost:8080/api/ws/notifications**

## API Endpoints

### Authentication
- `POST /api/auth/register` - Đăng ký tài khoản
- `POST /api/auth/login` - Đăng nhập
- `POST /api/auth/request-reset` - Yêu cầu đặt lại mật khẩu
- `POST /api/auth/reset-password` - Đặt lại mật khẩu
- `GET /api/auth/me` - Lấy thông tin người dùng hiện tại

### Comics
- `GET /api/comics/page?page=1` - Danh sách truyện (phân trang, 20 truyện/trang)
- `GET /api/comics/search?keyword=...` - Tìm kiếm truyện
- `GET /api/comics/{comicId}` - Chi tiết truyện
- `GET /api/comics/genre/{genreId}` - Truyện theo thể loại

### Chapters
- `GET /api/chapters/{chapterId}` - Chi tiết chapter
- `GET /api/chapters/comic/{comicId}` - Danh sách chapter của truyện

### Genres
- `GET /api/genres` - Danh sách thể loại
- `GET /api/genres/{genreId}` - Chi tiết thể loại

### Follows
- `POST /api/follows/user` - Theo dõi người dùng
- `DELETE /api/follows/user/{followedId}` - Bỏ theo dõi
- `POST /api/follows/comic` - Theo dõi truyện
- `DELETE /api/follows/comic/{comicId}` - Bỏ theo dõi truyện

### Crawl (Admin)
- `POST /api/crawl/latest?page=1` - Crawl danh sách truyện mới

### Notifications (WebSocket)
- `POST /api/notifications/send` - Gửi notification (broadcast)
- `POST /api/notifications/send-to-user` - Gửi notification cho user cụ thể
- WebSocket: `/ws/notifications` (STOMP + SockJS)

## Các thay đổi chính so với ASP.NET Core

### 1. Dependency Injection
```csharp
// C# .NET
builder.Services.AddScoped<AuthService>();
```

```java
// Java Spring Boot - tự động với @Service/@Component + @Autowired
@Service
public class AuthService { ... }
```

### 2. Database Context
```csharp
// C# .NET
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

```java
// Java Spring Boot - cấu hình trong application.yml
spring:
  datasource:
    url: jdbc:sqlserver://...
  jpa:
    hibernate:
      ddl-auto: validate
```

### 3. Entity Models
```csharp
// C# .NET - Attributes
[Table("account")]
[Column("account_id")]
public string AccountId { get; set; }
```

```java
// Java - Annotations
@Entity
@Table(name = "account")
@Column(name = "account_id")
private String accountId;
```

### 4. Controllers
```csharp
// C# .NET
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase { ... }
```

```java
// Java Spring Boot
@RestController
@RequestMapping("/api/accounts")
public class AccountController { ... }
```

### 5. Authentication & JW

T
```csharp
// C# .NET - Startup config
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opt => { ... });
```

```java
// Java Spring Boot - SecurityConfig
@Configuration
@EnableWebSecurity
public class SecurityConfig {
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        http.addFilterBefore(jwtAuthenticationFilter, UsernamePasswordAuthenticationFilter.class);
        return http.build();
    }
}
```

### 6. Async/Await
```csharp
// C# .NET
public async Task<IActionResult> GetComics() {
    var comics = await _context.Comics.ToListAsync();
    return Ok(comics);
}
```

```java
// Java Spring Boot - JPA handles basic queries, manual async with CompletableFuture if needed
public ResponseEntity<List<Comic>> getComics() {
    List<Comic> comics = comicRepository.findAll();
    return ResponseEntity.ok(comics);
}
```

### 7. SignalR → WebSocket
```csharp
// C# .NET
app.MapHub<NotificationHub>("/hubs/notifications");

public class NotificationHub : Hub {
    public async Task SendNotification(string message) {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }
}
```

```java
// Java Spring Boot - STOMP over WebSocket
@Controller
public class NotificationHandler {
    @MessageMapping("/notifications/send")
    @SendTo("/topic/notifications")
    public Map<String, Object> sendNotification(Map<String, Object> message) {
        return message;
    }
}
```

## Email Configuration (Gmail)

1. Kích hoạt "Less secure app access" hoặc tạo **App Password**:
   - Truy cập: https://myaccount.google.com/security
   - Tạo app-specific password

2. Cập nhật `application.yml`:
   ```yaml
   spring:
     mail:
       host: smtp.gmail.com
       port: 587
       username: your-email@gmail.com
       password: your-app-password
       properties:
         mail:
           smtp:
             auth: true
             starttls:
               enable: true
               required: true
   ```

## JWT Configuration

Cập nhật `application.yml`:
```yaml
jwt:
  key: ThisIsASecretKeyForJwtToken123456
  issuer: MyApp
  expiration: 86400000  # 24 hours in milliseconds
```

## Logging

Cập nhật log level trong `application.yml`:
```yaml
logging:
  level:
    root: INFO
    com.webtruyenapi: DEBUG
    org.springframework.web: DEBUG
    org.hibernate.SQL: DEBUG
```

## Testing

```bash
# Run tests
mvn test

# Run with coverage
mvn jacoco:report
```

## Troubleshooting

### 1. SQL Server Connection Error
- Kiểm tra URL: `jdbc:sqlserver://SERVER_NAME:1433;databaseName=database_name`
- Bật: `trustServerCertificate=true` nếu dùng self-signed certificate

### 2. JWT Token Expired
- Kiểm tra `jwt.expiration` trong `application.yml`

### 3. Email không gửi được
- Sử dụng App Password thay vì mật khẩu thường
- Bật SMTP authentication và TLS

### 4. CORS Issues
- Kiểm tra `allowedOrigins` trong `SecurityConfig.java`

## Future Improvements

- [ ] Thêm Unit Tests
- [ ] Thêm Integration Tests
- [ ] Redis caching
- [ ] Scheduled tasks (comic crawling)
- [ ] Pagination improvement
- [ ] Elasticsearch for better search
- [ ] Docker support
- [ ] CI/CD Pipeline

## Related Files

- Cấu hình: `src/main/resources/application.yml`
- Dependencies: `pom.xml`
- Main Application: `WebTruyenApiApplication.java`

## Support

Nếu gặp vấn đề, tham khảo:
- Spring Boot Docs: https://spring.io/projects/spring-boot
- Spring Data JPA: https://spring.io/projects/spring-data-jpa
- Spring Security: https://spring.io/projects/spring-security
- JWT Docs: https://github.com/jwtk/jjwt
