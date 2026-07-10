---
name: create-api
description: "Use when user asks to create a new API endpoint. Follows Clean Architecture (.NET 8)."
---

# Create API Endpoint

Base: c:\Users\phamq\OneDrive\Desktop\New folder (4)\Hackathon

## Checklist

- [ ] Step 0: Check entities and enums
- [ ] Step 0.5: Check user visibility rule — nếu API cho role khác admin (Student, Lecturer...), đảm bảo filter `IsDisable == false` để user bị disable ko xuất hiện. User bị ban (BanReason != null) vẫn visible. [[user-visibility-ban-vs-disable]]
- [ ] **Step 0.6: Tìm Admin API tương ứng** — nếu Admin đã có API cho entity này (list/detail/create/update/delete), dùng **response DTO của Admin làm chuẩn**. Copy field names, kiểu dữ liệu, cấu trúc. Chỉ sửa authorization và business logic khác nếu có yêu cầu riêng.
- [ ] **Step 0.7: Route cho GET detail** — chỉ cần ID của entity đó trong route. VD:
  - ✅ `rounds/{roundId}` (ko cần `events/{eventId}/rounds/{roundId}`)
  - ✅ `tracks/{trackId}` (ko cần `events/{eventId}/tracks/{trackId}`)
  - ✅ `awards/{awardId}` (ko cần `events/{eventId}/awards/{awardId}`)
  - ✅ `criteria-templates/{templateId}`
  - ❌ Không thêm parentId vào route detail nếu có thể lấy từ entity
- [ ] Step 1: Determine role and controller
- [ ] Step 2: Check/create controller
- [ ] Step 3: Check/create Repository
- [ ] Step 4: Check/create Repository interface
- [ ] Step 5: Check/create Application Service
- [ ] Step 6: Write use case logic
- [ ] Step 7: Handle enum fields
- [ ] Step 8: Helper check — nếu logic đã lặp lại ở ≥2 nơi (vd: pagination validation, date filter, ...), tách vào class ở `Hackathon.Application/Common/Helpers/` hoặc `Hackathon.Infrastructure/Helpers/`
- [ ] Step 9: Add success message (use SuccessMessage.cs)
- [ ] **Step 10: Write/update documentation ngay lập tức** — tạo file mới trong `documents/{role}/{entity}/{method}/` hoặc sửa file đã có nếu request/response thay đổi. KHÔNG ĐỢI NHẮC. **Phải viết chi tiết** (xem hướng dẫn bên dưới).
- [ ] Step 11: Register DI
- [ ] Step 12: Check exception handling — các lỗi có thể xảy ra (404 not found, 400 validation/enum, 409 conflict, 401, 403, 409 đã disable...)

---

## Phần 1: Flow các hành động

### START → [Yêu cầu tạo API]

```
1. Xác định Entity & Enum
2. Xác định Role → Controller (Admin/Staff/Lecturer/Student/Public)
3. Tạo Controller (Hackathon.Presentation/Controllers/)
4. Tạo Repository Interface & Implementation (nếu cần custom query)
5. Tạo Service Interface + Request + Response (Hackathon.Application/Services/{Entity}/)
6. Viết Business Logic trong Service
7. Xử lý Enum fields (body / query / route)
8. Helper check — logic nào lặp lại ≥2 nơi → tách helper (Application/Common/Helpers/ hoặc Infrastructure/Helpers/)
9. Kiểm tra & thêm ErrorMessage constants nếu thiếu
9. Kiểm tra & dùng SuccessMessage constants
10. Ghi doc: tạo file mới trong `documents/{role}/{entity}/{method}/` hoặc sửa file đã có nếu request/response thay đổi
11. Đăng ký DI (Application + Infrastructure)
12. Kiểm tra exception handling — các lỗi có thể xảy ra (404 not found, 400 validation/enum, 409 conflict, 401, 403, 409 đã disable, ...)
13. Build & verify
```

---

## Phần 2: Sơ đồ đi (Layer Mapping)

```
┌──────────────────────────────────────────────────────────────┐
│                     PRESENTATION LAYER                       │
│  Hackathon.Presentation/Controllers/                         │
│    └── {Role}Controller.cs                                    │
│        → injects I{Entity}Service (từ Services/{Entity}/)     │
│        → dùng ApiResponseFactory + SuccessMessage             │
│        → trả về IActionResult                                 │
│        → lấy HttpContext.Connection, Headers... pass vào DTO  │
└──────────────────────┬───────────────────────────────────────┘
                       │ gọi
                       ▼
┌──────────────────────────────────────────────────────────────┐
│                     APPLICATION LAYER                         │
│  Hackathon.Application/                                      │
│    ├── Services/{Entity}/                                     │
│    │    ├── I{Entity}Service.cs      ← interface service      │
│    │    ├── Service.cs               ← business logic         │
│    │    ├── Request.cs               ← input DTOs             │
│    │    └── Response.cs              ← output DTOs            │
│    │                                                          │
│    ├── Common/Interfaces/                                     │
│    │    ├── IJwtService.cs           ← shared services        │
│    │    ├── IPasswordService.cs                               │
│    │    ├── IMailService.cs                                   │
│    │    ├── IMediaService.cs                                  │
│    │    └── IUnitOfWork.cs                                    │
│    │                                                          │
│    ├── Common/IRepository/                                    │
│    │    ├── I{Entity}Repository.cs   ← repository interfaces  │
│    │    └── ...                                               │
│    │                                                          │
│    └── Common/                                               │
│         ├── Models/ApiResponse*.cs                            │
│         ├── Exceptions/ErrorMessage.cs                        │
│         └── SuccessMessage.cs                                 │
└──────────────────────┬───────────────────────────────────────┘
                       │ gọi interface (I{Entity}Repository)
                       ▼
┌──────────────────────────────────────────────────────────────┐
│                     INFRASTRUCTURE LAYER                      │
│  Hackathon.Infrastructure/                                    │
│    ├── Repositories/                                          │
│    │    ├── {Entity}Repository.cs     ← implements IRepository│
│    │    └── ...                                               │
│    ├── Services/                                              │
│    │    ├── Jwt/Service.cs                                    │
│    │    ├── Password/Service.cs                                │
│    │    ├── Mail/Service.cs                                    │
│    │    └── Cloudinary/Service.cs                              │
│    ├── AppDbContext.cs                                         │
│    ├── UnitOfWork.cs                                           │
│    └── DependencyInjection.cs                                 │
└──────────────────────────────────────────────────────────────┘
```

---

## Phần 3: Chi tiết từng folder — quy tắc đặt tên, cấu trúc bên trong

### 3.1. Presentation Layer — Controllers

**Folder:** `Hackathon.Presentation/Controllers/`

**Quy tắc đặt tên:**
- File: `{Role}{Entity}Controller.cs` (vd: `AdminUserController.cs`, `AdminEventController.cs`, `AuthController.cs`)
- Route: `[Route("api/v1/admin")]` cho admin, `[Route("api/v1/{controller}")]` cho public
- Class: kế thừa `ControllerBase`, attribute `[ApiController]`
- **Mỗi thực thể lớn (entity) có controller riêng** — không gộp nhiều entity vào 1 controller. VD: AdminUserController cho user, AdminEventController cho event, AdminTeamController cho team, AdminRegisterTeamController cho register-team, ...

**Cấu trúc bên trong:**
```csharp
using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.{Entity};
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers;

[Route("api/v1/{controller}")]
[ApiController]
public class {Role}Controller : ControllerBase
{
    private readonly I{Entity}Service _{entity}Service;

    public {Role}Controller(I{Entity}Service {entity}Service)
    {
        _{entity}Service = {entity}Service;
    }

    [HttpPost("{action}")]
    public async Task<IActionResult> {Action}([FromBody] {Action}Request request)
    {
        // Lấy thông tin HttpContext gán vào request nếu cần
        // request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        // request.UserAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var result = await _{entity}Service.{Action}(request);
        return Ok(ApiResponseFactory.Success(
            result,
            message: SuccessMessage.{Category}.{Name},
            status: 201,
            traceId: HttpContext.TraceIdentifier
        ));
    }

    [HttpGet("{action}")]
    public async Task<IActionResult> {Action}([FromQuery] {Action}Request request)
    {
        var result = await _{entity}Service.{Action}(request);
        return Ok(ApiResponseFactory.Success(
            result,
            message: SuccessMessage.{Category}.{Name},
            traceId: HttpContext.TraceIdentifier
        ));
    }
}
```

**Lưu ý:**
- Không dùng hậu tố `Async` trong tên method (vd: `Login` thay vì `LoginAsync`)
- `using` chỉ cần `Hackathon.Application.Services.{Entity}` — namespace chứa cả interface và DTOs
- `SuccessMessage` ở `Hackathon.Application.Common`
- Chỉ Controller mới được lấy từ `HttpContext` — không truyền `IHttpContextAccessor` vào Application
- **Convention HTTP methods:**
  - **GET** — chỉ lấy dữ liệu (danh sách, chi tiết, đếm, ...)
  - **POST** — tạo mới, và các hành động khác (delete, restore, swap, approve, reject, ban, unban, assign track, ...)
  - **PATCH** — chỉ update/chỉnh sửa thông tin
  - Không dùng `[HttpDelete]` — xóa mềm dùng `POST .../{id}/delete`
  - **GET response luôn trả về ID của entity gốc** — thừa còn hơn thiếu, tránh FE thiếu field phải hỏi lại

---

### 3.2. Application Layer — Services

**Folder:** `Hackathon.Application/Services/{Entity}/`

**Quy tắc đặt tên:**
| File | Mục đích | Ví dụ |
|------|----------|-------|
| `I{Entity}Service.cs` | Interface service | `IAuthService.cs` |
| `Service.cs` | Implementation | `Service.cs` |
| `Request.cs` | Input DTOs (tất cả request cho entity này) | `Request.cs` |
| `Response.cs` | Output DTOs (tất cả response cho entity này) | `Response.cs` |

**Quy tắc method name:** Không hậu tố `Async`. VD: `Login`, `Register`, `GetById`, `Create`.

**Cấu trúc folder Services (từ bản refactor):**

Services được chia làm 2 nhóm chính:

```
Hackathon.Application/Services/
├── Admin/                          ← API cho Admin
│   ├── Award/
│   ├── Event/
│   ├── Notification/
│   ├── User/                       ← admin user ops (CRUD, ban, ...)
│   ├── ... (mỗi entity 1 folder)
│   └── DependencyInjection.cs      ← AddAdminServices()
└── Base/                           ← API cho user thường
    ├── Auth/
    ├── User/                       ← hồ sơ cá nhân (GetMyProfile)
    └── DependencyInjection.cs      ← AddBaseServices()
```

**Quy tắc:**
- **Tên folder = số ít** — `Award` không phải `Awards`, `Event` không phải `Events`, `User` không phải `Users`. Dùng tên entity (không số nhiều) tránh conflict namespace.
- **Controller nào → Service đó:** `AdminUserController` → dùng `Admin/User/`, `UserController` (user thường) → dùng `Base/User/`, `AuthController` → dùng `Base/Auth/`.
- **Mỗi nhóm có DI riêng:** `Admin/DependencyInjection.cs` chứa `AddAdminServices()`, `Base/DependencyInjection.cs` chứa `AddBaseServices()`. Root DI (`Application/DependencyInjection.cs`) gọi cả 2.
- **User visibility:** API cho role ≠ Admin → filter `IsDisable == false`. User bị ban (`BanReason != null`) vẫn visible với mọi role. [[user-visibility-ban-vs-disable]]

**Interface:**
```csharp
namespace Hackathon.Application.Services.Admin.{Entity};
```csharp
namespace Hackathon.Application.Services.{Entity};

public interface I{Entity}Service
{
    Task<{Action}Response> {Action}({Action}Request request);
}
```

**Cấu trúc Service:**
```csharp
using Hackathon.Application.Common.Interfaces;    // shared services
using Hackathon.Application.Common.IRepository;   // repository interfaces
using Hackathon.Application.Exceptions;           // ErrorMessage
using Hackathon.Domain.Entities;                 // Domain entities
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;
using SuccMsg = Hackathon.Application.Common.SuccessMessage;

namespace Hackathon.Application.Services.{Entity};

public class Service : I{Entity}Service
{
    private readonly I{Entity}Repository _{entity}Repository;
    private readonly IUnitOfWork _unitOfWork;
    // + các shared service (IJwtService, IPasswordService, ...)

    public Service(
        I{Entity}Repository {entity}Repository,
        IUnitOfWork unitOfWork /*, ... */)
    {
        _{entity}Repository = {entity}Repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<{Action}Response> {Action}({Action}Request request)
    {
        // 1. Validation / business rules
        // 2. Call repository
        // 3. _unitOfWork.SaveChangesAsync()
        // 4. Map & return Response DTO
    }
}
```

**Request DTO — quy tắc:**
- Dùng `[Required]`, `[EmailAddress]`, `[StringLength]` attributes
- Field không bắt buộc → nullable (`string?`, `int?`)
- Nếu cần thông tin từ HttpContext (IpAddress, UserAgent) → thêm field optional

**Response DTO — quy tắc:**
- Chỉ chứa dữ liệu trả về
- Field không có → nullable (`string?`)
- AccessToken + RefreshToken response dùng chung pattern

**Lưu ý:**
- Không dùng `IHttpContextAccessor` trong Application layer
- Không dùng DbContext, EF Core ở đây — qua Repository interface
- Throw `NotFoundException`, `BadRequestException`, `ConflictException` qua `ErrorMessage`
- Auth policy claims trong JWT: **chỉ chứa `UserId`** — không đẩy Role, IsVerified hay field khác

---

### 3.3. Application Layer — Common/IRepository

**Folder:** `Hackathon.Application/Common/IRepository/`

**Quy tắc đặt tên:** `I{Entity}Repository.cs` — mỗi entity một file.

**Cấu trúc:**
```csharp
using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface I{Entity}Repository
{
    // CRUD methods — chỉ định nghĩa nếu cần custom query
    Task<{Entity}?> GetByIdAsync(Guid id);
    Task AddAsync({Entity} entity);
    Task UpdateAsync({Entity} entity);
}
```

**Lưu ý:** Chỉ tạo khi cần custom query. Generic CRUD cơ bản có thể dùng pattern riêng.

---

### 3.4. Application Layer — Common/Interfaces

**Folder:** `Hackathon.Application/Common/Interfaces/`

Chỉ chứa interface của các **shared/dùng chung services**:

| File | Mô tả |
|------|-------|
| `IJwtService.cs` | JWT token generation + validation |
| `IPasswordService.cs` | Hash + verify password |
| `IMailService.cs` | Gửi email |
| `IMediaService.cs` | Upload file/image |
| `IUnitOfWork.cs` | SaveChanges |

Các interface này không liên quan đến entity cụ thể, dùng chung toàn bộ application.

---

### 3.5. Application Layer — Exceptions & Messages

| File | Mô tả | Format |
|------|-------|--------|
| `ErrorMessage.cs` | Lỗi — `throw new BadRequestException(ErrMsg.Auth.UserNotFound)` | Capitalize Each Word |
| `SuccessMessage.cs` | Thành công — `SuccessMessage.Auth.LoginSuccessful` | Capitalize Each Word |

---

### 3.6. Infrastructure Layer — Repositories

**Folder:** `Hackathon.Infrastructure/Repositories/`

**Quy tắc đặt tên:** `{Entity}Repository.cs` — implements `I{Entity}Repository` từ `Common/IRepository/`.

```csharp
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class {Entity}Repository : I{Entity}Repository
{
    private readonly AppDbContext _context;

    public {Entity}Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<{Entity}?> GetByIdAsync(Guid id)
        => await _context.{Entities}.FindAsync(id);

    public async Task AddAsync({Entity} entity)
        => await _context.{Entities}.AddAsync(entity);

    public Task UpdateAsync({Entity} entity)
    {
        _context.{Entities}.Update(entity);
        return Task.CompletedTask;
    }
}
```

---

### 3.7. DI Registration

**Infrastructure (`Hackathon.Infrastructure/DependencyInjection.cs`):**
```csharp
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;

services.AddScoped<I{Entity}Repository, Repositories.{Entity}Repository>();
```

**Application (`Hackathon.Application/DependencyInjection.cs`):**
```csharp
using Hackathon.Application.Services.{Entity};

services.AddScoped<I{Entity}Service, Services.{Entity}.Service>();
```

---

## Phần 4: Hướng dẫn viết documentation CHI TIẾT

### Nguyên tắc chung

Doc API phải được viết **chi tiết, đầy đủ ngữ cảnh nghiệp vụ** để người đọc (FE dev, tester, người mới) hiểu ngay API làm gì, ai dùng được, khi nào dùng.

**Tiêu chí bắt buộc:**
1. **Mô tả nghiệp vụ bằng ngôn ngữ người dùng**: Viết như kiểu "Người dùng muốn xem danh sách...", "Staff cần lấy các event mà họ được phân công để..."
2. **Giải thích ngữ cảnh**: Tại sao cần API này? Nó giải quyết vấn đề gì?
3. **Mô tả luồng xử lý**: Controller → Service → Repository đã làm những bước gì (xác thực, kiểm tra quyền, filter gì, sắp xếp ra sao...)
4. **Ghi rõ các điều kiện ẩn**: VD: "mặc định không lấy status Draft", "chỉ lấy IsDisable=false", "sắp xếp theo CreatedAt DESC"

### Cấu trúc file doc

```markdown
# METHOD /api/v1/{role}/{path}

> Một câu tóm tắt ngắn gọn (ai làm gì).

## Nghiệp vụ

Mô tả chi tiết bằng ngôn ngữ người dùng:
- Người dùng muốn làm gì?
- Ngữ cảnh sử dụng: khi nào họ gọi API này?
- Luồng nghiệp vụ: hệ thống xử lý những bước gì?
- Các điều kiện ẩn: filter gì mặc định, sắp xếp ra sao, loại trừ gì?

## Phân quyền
- ✅ Role nào được dùng (Admin / Staff / Student / Lecturer / Public)
- Các điều kiện phụ (phải được assign, phải là chủ sở hữu...)

## Request

### Route Parameters (nếu có)
| Parameter | Type | Description |
|-----------|------|-------------|
| id | Guid | Mô tả cụ thể |

### Query Parameters (nếu có)
| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| keyword | string | No | - | Giải thích rõ tìm kiếm theo trường gì |

### Body (nếu có)
```json
{ "field": "value" }
```

## Response (200)
```json
{
  "data": { ... },
  "message": "...",
  "status": 200,
  "traceId": "..."
}
```

### Field ý nghĩa

| Field | Ý nghĩa |
|-------|---------|
| fieldName | Giải thích rõ field này là gì, có thể null không |

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | ... | Token hết hạn/thiếu |
| 403 | ... | Không có quyền / không được assign |
| 404 | ... | ID không tồn tại hoặc đã bị disable |
```

### Ví dụ doc tốt vs doc kém

**❌ Kém (quá ngắn, thiếu ngữ cảnh):**
```markdown
# GET /api/v1/staff/events

> Lấy danh sách event

## Request
| Parameter | Type | Description |
|-----------|------|-------------|
| pageIndex | int | Trang |
```

**✅ Tốt (chi tiết, có ngữ cảnh nghiệp vụ):**
```markdown
# GET /api/v1/staff/events

> Staff đã đăng nhập lấy danh sách các event mà họ được phân công, có filter và phân trang.

## Nghiệp vụ

Staff muốn xem tất cả event họ được gán làm việc (qua bảng AssignEvents). 
API này giúp staff nhanh chóng tìm event cần làm việc.

- Mặc định loại trừ event có status Draft: staff chỉ thấy event đã Published hoặc Closed.
- Hỗ trợ tìm kiếm theo từ khóa (tên event), lọc theo status, và khoảng thời gian.
- Kết quả sắp xếp theo ngày tạo event mới nhất trước.

## Phân quyền
- ✅ Staff (RoleEnum = Staff)
```

---

## Tham chiếu mẫu

Xem các file sau để tham khảo cách implement:

| File | Chức năng |
|------|-----------|
| `Services/Auth/IAuthService.cs` | Service interface mẫu |
| `Services/Auth/Service.cs` | Service implementation mẫu (Login, Register, VerifyEmail) |
| `Services/Auth/Request.cs` | Request DTOs mẫu |
| `Services/Auth/Response.cs` | Response DTOs mẫu |
| `Common/IRepository/IUserRepository.cs` | Repository interface mẫu |
| `Infrastructure/Repositories/UserRepository.cs` | Repository implementation mẫu |
| `Controllers/AuthController.cs` | Controller mẫu |
| `Common/Interfaces/IJwtService.cs` | Shared service interface mẫu |