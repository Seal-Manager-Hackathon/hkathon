# Cách Hoạt Động & Sơ Đồ Di Dời Clean Architecture

> ⚠️ **Quan trọng:** Trước khi thêm cái mới hoặc thay đổi cấu trúc, hãy đọc file này để thảo luận. Mọi thay đổi lớn cần được check lại và xác nhận trước khi thực hiện.

---

## 🏛 Kiến Trúc Tổng Thể (Clean Architecture 4 Tầng)

```
Hackathon.sln
│
├── Hackathon.Domain           # Tầng LÕI (Core) - không ref ai
├── Hackathon.Application      # Tầng NGHIỆP VỤ - chỉ ref Domain
├── Hackathon.Infrastructure   # Tầng HẠ TẦNG - ref Application + Domain
└── Hackathon.Presentation     # Tầng API - ref Application + Infrastructure
```

**Chiều tham chiếu:**
```
Presentation ────> Application ────> Domain
     │                  │               │
     └──────────> Infrastructure ───────┘
```

---

## 📁 1. Hackathon.Domain (Lõi — Core Business)

> **Tại sao để ở đây:** Domain là tầng không phụ thuộc gì cả (no dependency). Nó chứa các quy tắc nghiệp vụ cốt lõi — entity, enum, exception, interface. Các tầng khác đều ref vào Domain.

```
Hackathon.Domain/
├── Entities/
│   ├── BaseEntity.cs         ← CŨ: Repository/Abtraction/
│   ├── IAuditableEntity.cs   ← CŨ: Repository/Abtraction/
│   ├── Users.cs              ← CŨ: Repository/Entity/
│   ├── Events.cs             ← CŨ: Repository/Entity/
│   ├── Teams.cs              ← CŨ: Repository/Entity/
│   ├── ... (27 entity files) ← CŨ: Repository/Entity/
│
├── Enums/
│   ├── User/                 ← CŨ: Repository/Enum/RoleEnum, UserStatusEnum
│   ├── Event/                ← CŨ: Repository/Enum/EventStatusEnum, SeasonEnum
│   ├── EventRole/            ← CŨ: Repository/Enum/EventRoleEnum
│   ├── RegisterTeam/         ← CŨ: Repository/Enum/RegisterTeamStatusEnum
│   ├── Invitation/           ← CŨ: Repository/Enum/InvitationStatusEnum
│   ├── Submission/           ← CŨ: Repository/Enum/SubmissionStatusEnum
│   ├── Score/                ← CŨ: Repository/Enum/ScoresStatusEnum
│   ├── LeaderBoard/          ← CŨ: Repository/Enum/LeaderBoardsStatusEnum
│   ├── Notification/         ← CŨ: Repository/Enum/NotificationStatus*, NotificationTargetType*
│   ├── EmailVerification/    ← CŨ: Repository/Enum/EmailVerificationStatusEnum
│   ├── Report/               ← CŨ: Repository/Enum/ReportStatusEnum
│   └── TeamDetail/           ← CŨ: Repository/Enum/TeamDetailStatusEnum
│
├── Exceptions/
│   ├── AppException.cs           ← CŨ: Service/Exceptions/
│   ├── AuthExceptions.cs         ← CŨ: Service/Exceptions/
│   ├── CommonExceptions.cs       ← CŨ: Service/Exceptions/
│   └── LeaderBoardExceptions.cs  ← CŨ: Service/Exceptions/
│
└── Interfaces/
    ├── BaseEntity.cs         ← Base class abstract: Id, IsDisable
    ├── IAuditableEntity.cs   ← Interface: CreatedAt, UpdatedAt
    └── IRepository.cs        ← Interface generic repository (tạm thời chưa xài)
```

### Cách dùng:
- **Entities** để Application service truy vấn và thao tác qua DbContext
- **Enums** được các entity reference (VD: `Users.Role` là `RoleEnum`)
- **Exceptions** được throw từ Application service, Presentation/Filters bắt lại trả HTTP response
- **Interfaces** (BaseEntity, IAuditableEntity) là base cho tất cả entities

---

## 📁 2. Hackathon.Application (Logic nghiệp vụ — Business Logic)

> **Tại sao để ở đây:** Tầng này chứa toàn bộ xử lý nghiệp vụ. Tách riêng khỏi API (Presentation) giúp dễ test, dễ thay đổi logic mà ko ảnh hưởng API. Tách khỏi Infrastructure giúp không phụ thuộc EF Core hay thư viện bên thứ 3.

```
Hackathon.Application/
├── Services/
│   ├── Auths/                   ← CŨ: Service/Auths/
│   │   ├── IService.cs          ← Interface các method auth
│   │   ├── Service.cs           ← Implement: Login, Register, RefreshToken...
│   │   ├── Request.cs           ← DTO: LoginRequest, RegisterRequest...
│   │   └── Response.cs          ← DTO: AuthResponse, TokenResponse...
│   │
│   ├── Admin/                   ← CŨ: Service/Admin/
│   │   ├── IService.cs          ← CRUD user, role, rounds cho Admin
│   │   ├── Service.cs
│   │   ├── Request.cs
│   │   └── Response.cs
│   │
│   ├── Events/                  ← CŨ: Service/Events/  (28 endpoints, lớn nhất)
│   │   ├── IService.cs          ← Public: GetEvents, GetJoinedEvents
│   │   │                        ← Admin: CreateEvent, UpdateEvent, Publish...
│   │   │                        ← Staff: AssignLecturer, RecalcLeaderboard...
│   │   ├── Service.cs
│   │   ├── Request.cs
│   │   └── Response.cs
│   │
│   ├── Rounds/                  ← CŨ: Service/Rounds/
│   │   ├── IService.cs
│   │   ├── Service.cs           ← Gồm cả static CloseAndAdvanceRoundAsync (Quartz job)
│   │   ├── Request.cs
│   │   └── Response.cs
│   │
│   ├── Tracks/                  ← CŨ: Service/Tracks/
│   ├── Teams/                   ← CŨ: Service/Teams/
│   ├── Submissions/             ← CŨ: Service/Submissions/
│   ├── RegisterTeams/           ← CŨ: Service/RegisterTeams/
│   ├── Invitations/             ← CŨ: Service/Invitations/
│   ├── Judges/                  ← CŨ: Service/Judges/
│   ├── Mentors/                 ← CŨ: Service/Mentors/
│   ├── Lecturers/               ← CŨ: Service/Lecturers/
│   ├── Staff/                   ← CŨ: Service/Staff/
│   ├── LeaderBoards/            ← CŨ: Service/LeaderBoards/
│   ├── Notifications/           ← CŨ: Service/Notifications/
│   ├── Criticals/               ← CŨ: Service/Criticals/
│   ├── AssignEvents/            ← CŨ: Service/AssignEvents/
│   ├── AssignTracks/            ← CŨ: Service/AssignTracks/
│   ├── Topics/                  ← CŨ: Service/Topics/
│   ├── Roles/                   ← CŨ: Service/Roles/
│   ├── Systems/                 ← CŨ: Service/Systems/
│   └── Users/                   ← CŨ: Service/Users/
│
├── Models/                      ← CŨ: Service/Models/
│   ├── BasePaginationResponse.cs ← Response phân trang chung
│   └── ApiResponseFactory.cs     ← Factory tạo Api response
│
├── Validations/                 ← CŨ: Service/Validations/
│   └── *.cs                     ← FluentValidation validators
│
└── Mappings/                    ← (trống, ko xài AutoMapper)
```

### Cách dùng:
- Mỗi entity có 1 folder riêng với 4 files: **IService** (interface), **Service** (impl), **Request**, **Response**
- **IService** có comment `#{Role}` để biết endpoint nào cho role nào
- **Service** gọi `AppDbContext` (từ Infrastructure) để query, throw `Domain.Exceptions` khi có lỗi
- **Models/** chứa response wrapper dùng chung toàn hệ thống

---

## 📁 3. Hackathon.Infrastructure (Hạ tầng — Data, Third-party, Background Jobs)

> **Tại sao để ở đây:** Tầng này chứa các cài đặt kỹ thuật và tích hợp bên thứ 3. Tách riêng giúp Application không bị phụ thuộc vào EF Core, MailKit, Cloudinary — nếu đổi thư viện chỉ cần sửa Infrastructure.

```
Hackathon.Infrastructure/
├── Data/
│   ├── AppDbContext.cs               ← CŨ: Repository/AppDbContext.cs
│   │                                  DbContext EF Core: DbSet cho 27 entities
│   │                                  + OnModelCreating: Fluent API + Seed
│   │
│   ├── Migrations/                   ← CŨ: Repository/Migrations/
│   │   ├── 20260703162045_Initial.cs
│   │   ├── 20260703162045_Initial.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   │
│   └── Seed/
│       ├── 01/                       ← CŨ: Repository/Seed/ (trừ Demo + FPT)
│       │   ├── SeedConstants.cs      ← Constants: ID mặc định cho roles, users
│       │   ├── UserSeed.cs           ← Users: Admin, Staff, Student mẫu
│       │   ├── EventRoleSeed.cs      ← EventRoles: Mentor, Judge, Staff
│       │   ├── EventSeed.cs          ← Events mẫu
│       │   ├── RoundSeed.cs          ← Rounds mẫu
│       │   ├── TrackSeed.cs          ← Tracks mẫu
│       │   ├── AuthSeed.cs           ← EmailVerifications mẫu
│       │   ├── AwardSeed.cs          ← Awards mẫu
│       │   ├── CriteriaSeed.cs       ← CriteriaTemplates + Items
│       │   ├── TeamSeed.cs           ← Teams + TeamDetails + RegisterTeams
│       │   ├── RoundDetailSeed.cs    ← RoundDetails
│       │   ├── SubmissionSeed.cs     ← Submissions
│       │   ├── AssignmentSeed.cs     ← AssignEvents + AssignTracks
│       │   ├── ScoreSeed.cs          ← Scores + ScoreItems
│       │   ├── LeaderBoardSeed.cs    ← LeaderBoards + Details
│       │   ├── NotificationSeed.cs   ← Notifications
│       │   └── ReportSeed.cs         ← Reports
│       │
│       └── 02/                       ← CŨ: Repository/Seed/DemoSeed + FPTSeed
│           ├── DemoSeed.cs           ← Seed demo data cơ bản
│           └── FPTSeed.cs            ← Seed 60 FPT students + 10 lecturers + teams
│
├── Services/
│   ├── JwtServices/                  ← CŨ: Service/JwtServices/
│   │   ├── JwtService.cs            ← Tạo JWT token
│   │   ├── IJwtService.cs           ← Interface
│   │   └── JwtOptions.cs            ← Config model
│   │   → Sử dụng bởi Auths service (Application) để tạo token khi login
│   │
│   ├── MailServices/                ← CŨ: Service/MailServices/
│   │   ├── IMailService.cs
│   │   ├── MailService.cs           ← Gửi mail SMTP Gmail (MailKit)
│   │   ├── MailRequest.cs
│   │   └── MailOptions.cs
│   │   → Sử dụng bởi Auths service (gửi email verify, reset password)
│   │
│   ├── CloudinaryService/           ← CŨ: Service/CloudinaryService/
│   │   ├── ICloudinaryService.cs
│   │   └── CloudinaryService.cs
│   │   → Upload ảnh lên Cloudinary (avatar, submission img)
│   │
│   ├── MediaService/                ← CŨ: Service/MediaService/
│   │   ├── IMediaService.cs
│   │   └── MediaService.cs
│   │   → Xử lý media files (resize, validate)
│   │
│   ├── BackgroundJobService/        ← CŨ: Service/BackgroundJobService/
│   │   ├── EndRoundJob.cs           ← Quartz job: đóng round + chuyển tiếp
│   │   ├── AutoRejectPendingRegistrationsJob.cs  ← Auto từ chối đăng ký quá hạn
│   │   └── ExpirePendingInvitationsJob.cs        ← Hết hạn lời mời
│   │
│   └── Localization/                ← CŨ: Service/Localization/
│       └── JsonStringLocalizer.cs   ← Đa ngôn ngữ
│
└── Repositories/                    ← (tuỳ chọn, nếu cần implement IRepository từ Domain)
```

### Cách dùng:
- **AppDbContext** được inject vào Application Service
- **Migrations** dùng để cập nhật database schema
- **Seed/01** chạy mặc định, **Seed/02** chạy khi cần data demo
- **JwtServices, MailServices, CloudinaryService** được inject vào Application Service
- **BackgroundJobService** dùng Quartz chạy job định kỳ

---

## 📁 4. Hackathon.Presentation (API — ASP.NET Web API)

> **Tại sao để ở đây:** Tầng này chịu trách nhiệm giao tiếp HTTP. Controllers gọi Application services. Tách riêng giúp có thể thay đổi API (Rest → GraphQL, gRPC) mà không ảnh hưởng logic.

```
Hackathon.Presentation/
├── Controllers/                     ← CŨ: Api/Controllers/
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── EventsController.cs
│   ├── TeamsController.cs
│   ├── SubmissionsController.cs
│   ├── RegisterTeamController.cs
│   ├── InvitationsController.cs
│   ├── JudgesController.cs
│   ├── MentorsController.cs
│   ├── LecturersController.cs
│   ├── Staff.cs
│   ├── LeaderBoardsController.cs
│   ├── NotificationsController.cs
│   ├── CriticalsController.cs
│   ├── RolesController.cs
│   ├── RoundsController.cs
│   ├── TracksController.cs
│   ├── TopicsController.cs
│   ├── UserController.cs
│   └── SystemController.cs
│
├── Extensions/                     ← CŨ: Api/Extention/
│   ├── JwtExtensions.cs           ← JWT Bearer authentication
│   ├── ServiceExtensions.cs       ← Đăng ký DI
│   ├── SwaggerExtensions.cs       ← Swagger/OpenAPI
│   └── CorsExtensions.cs          ← CORS
│
├── Middleware/                     ← CŨ: Api/Middleware/
│   └── GlobalExceptionMiddleware.cs ← Bắt exception → JSON response
│
├── Filters/                       ← CŨ: Api/Filters/
│   └── ValidationFilter.cs        ← Validate ModelState
│
├── Localization/
├── Resources/
├── Program.cs
├── appsettings.json
└── Dockerfile
```

---

## 🔄 Flow Request Chi Tiết

```
Client
  → [Middleware] GlobalExceptionMiddleware
  → [Filters] ValidationFilter
  → [Controller] gọi Service
  → [Application] Business Logic
      → [Domain] Entities + Exceptions
      → [Infrastructure] DbContext + Services
  → Response về Client
```

---

## ✅ Checklist Khi Di Dời

- [ ] File cũ và mới đã copy đúng nội dung?
- [ ] Namespace đã update?
- [ ] Build 0 errors?
- [ ] Đã xóa file cũ (hoặc backup _Check.)?
- [ ] GitNexus re-index?
- [ ] Nếu thêm mới: thảo luận trước khi làm?
