# AGENT INSTRUCTION — Clean Architecture .NET 8 Web API Convention

> Tài liệu này dùng để AI Agent (Claude Code, Copilot, hoặc bất kỳ coding agent nào) đọc và tuân theo khi dựng/scaffold project theo đúng convention đã thống nhất. Agent PHẢI tuân thủ nghiêm ngặt các quy tắc dưới đây, không tự ý đổi cấu trúc trừ khi được yêu cầu rõ ràng.

---

## 0. Tổng quan kiến trúc

Dự án áp dụng **Clean Architecture** với 4 project, KHÔNG dùng MediatR/CQRS (trừ khi được yêu cầu thêm sau), auth dùng `[Authorize]` thuần ASP.NET Core.

```
MyApp.sln
├── src/
│   ├── MyApp.Domain/           (không reference project nào)
│   ├── MyApp.Application/      (reference: Domain)
│   ├── MyApp.Infrastructure/   (reference: Application, Domain)
│   └── MyApp.WebApi/           (reference: Application, Infrastructure)

```

**Chiều reference (bắt buộc tuân thủ):**

```
WebApi ──► Application ──► Domain
   │                          ▲
   └──► Infrastructure ───────┘
```

- `Infrastructure` PHẢI reference `Application` (để implement interface) và `Domain` (để dùng Entity, cấu hình EF Core).
- `WebApi` reference `Infrastructure` CHỈ để phục vụ đăng ký DI trong `Program.cs`. Controller KHÔNG BAO GIỜ được gọi thẳng class trong Infrastructure.

---

## 1. QUY TẮC DTO — QUAN TRỌNG NHẤT, ÁP DỤNG XUYÊN SUỐT DỰ ÁN

> **Toàn bộ Request và Response đều là DTO, đặt DUY NHẤT ở tầng `Application`. KHÔNG tạo bất kỳ class Request/Response riêng nào ở `WebApi` (Presentation).**

### 1.1. Vị trí

```
MyApp.Application/
└── DTOs/
    └── Events/
        ├── CreateEventDto.cs      # input (tạo mới)
        ├── UpdateEventDto.cs      # input (cập nhật)
        ├── EventDto.cs            # output (dùng chung cho GET, và làm base cho role khác)
        └── AdminEventDto.cs       # output (kế thừa EventDto, thêm field riêng cho Admin)
```

### 1.2. Quy tắc phân biệt input/output

- **Input DTO**: đặt tên `Create{Entity}Dto`, `Update{Entity}Dto`... — dùng làm tham số của method trong Application Service, ĐỒNG THỜI dùng thẳng làm kiểu tham số của Controller action (không map sang class khác).
- **Output DTO**: đặt tên `{Entity}Dto` — dùng làm kiểu trả về của Application Service, ĐỒNG THỜI dùng thẳng làm kiểu trả về của Controller action (không map sang class khác).

### 1.3. Validate hình thức đặt NGAY TRÊN input DTO, dùng Data Annotations thuần .NET

```csharp
// Application/DTOs/Events/CreateEventDto.cs
using System.ComponentModel.DataAnnotations;

public class CreateEventDto
{
    [Required]
    public string Title { get; set; }

    [Required]
    public DateTime StartDate { get; set; }
}
```

- CHỈ dùng annotation thuộc `System.ComponentModel.DataAnnotations` (`[Required]`, `[Range]`, `[EmailAddress]`, `[MaxLength]`...).
- TUYỆT ĐỐI KHÔNG dùng annotation đặc thù ASP.NET (`[FromBody]`, `[FromQuery]`, `[FromForm]`, `IFormFile`) trên DTO trong Application — nếu 1 endpoint bắt buộc cần các annotation này (ví dụ upload file), đó là NGOẠI LỆ DUY NHẤT được phép tạo Request riêng ở WebApi cho đúng endpoint đó, và phải nói rõ lý do trong code comment.

### 1.4. Ví dụ Controller — dùng thẳng DTO, không map

```csharp
// WebApi/Controllers/EventsController.cs
[HttpPost]
public async Task<IActionResult> Create(CreateEventDto input, CancellationToken ct)
{
    var eventDto = await _eventService.CreateEventAsync(input, ct);
    return CreatedAtAction(nameof(GetById), new { id = eventDto.Id }, eventDto);
}

[HttpGet("{id}")]
public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
{
    var eventDto = await _eventService.GetByIdAsync(id, ct);
    return eventDto is null ? NotFound() : Ok(eventDto);
}
```

### 1.5. Trường hợp nhiều output khác nhau theo role (ví dụ Admin thấy nhiều field hơn Student)

Tách bằng KẾ THỪA ngay từ DTO, KHÔNG tách ở Response:

```csharp
// Application/DTOs/Events/EventDto.cs — base, dùng cho Student
public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
}

// Application/DTOs/Events/AdminEventDto.cs — thêm field CHỈ Admin thấy
public class AdminEventDto : EventDto
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
```

Controller chọn DTO theo role (đọc role ở đây, KHÔNG đẩy khái niệm "role" xuống Application/Infrastructure):

```csharp
[HttpGet]
[Authorize]
public async Task<IActionResult> GetEvents(CancellationToken ct)
{
    if (User.IsInRole("Admin"))
        return Ok(await _eventService.GetEventsForAdminAsync(ct));

    return Ok(await _eventService.GetEventsForStudentAsync(ct));
}
```

---

## 2. QUY TẮC CÁC TẦNG (nhắc lại để agent bám sát khi sinh code)

| Tầng                      | Chứa gì                                                                                                                                                                                 | Không được chứa gì                                                                              |
| ------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------- |
| **Domain**                | Entity, Value Object, Domain Exception, Enum                                                                                                                                            | Bất kỳ thứ gì liên quan EF Core, ASP.NET, DTO                                                   |
| **Application**           | Service (use case), DTO (input + output, GỘP CHUNG, không tách Request/Response riêng), Interface (`IXxxRepository`, `IUnitOfWork`), Application Exception (`NotFoundException`...)     | Annotation ASP.NET-specific, code EF Core trực tiếp                                             |
| **Infrastructure**        | DbContext, Repository implementation (chỉ nhận/trả **Domain Entity**, KHÔNG BAO GIỜ nhận/trả DTO hay Request/Response), UnitOfWork implementation, external service (email, payment...) | Logic nghiệp vụ, khái niệm "role"/"quyền", DTO                                                  |
| **Presentation (WebApi)** | Controller (mỏng — chỉ gọi Application Service và trả kết quả), Middleware xử lý exception, cấu hình `[Authorize]`                                                                      | Request/Response riêng (xem mục 1), logic nghiệp vụ, gọi thẳng Infrastructure để xử lý business |

### Nguyên tắc Repository — luôn tham số hóa, KHÔNG tạo method riêng theo role

```csharp
// Application/Common/Interfaces/IEventRepository.cs
public interface IEventRepository
{
    Task<List<Event>> GetAllAsync(bool includeDeleted, CancellationToken ct);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Event entity, CancellationToken ct);   // CHỈ đánh dấu, KHÔNG SaveChanges bên trong
}
```

Repository KHÔNG được biết "Admin"/"Student" là gì — chỉ nhận cờ boolean (`includeDeleted`) hoặc tham số kỹ thuật thuần túy.

---

## 3. QUY TẮC SAVECHANGES / TRANSACTION

1. `Add()`/`Update()`/`Remove()` trong Repository **CHỈ đánh dấu (tracking)**, KHÔNG bao giờ tự gọi `SaveChanges()` bên trong Repository.
2. Application Service gọi `IUnitOfWork.SaveChangesAsync()` **đúng 1 lần ở cuối method**, sau khi đã gọi hết các `Add()`/`Update()` cần thiết — vì 1 lần `SaveChangesAsync()` đã tự động là 1 transaction ngầm bao trọn mọi thay đổi.
3. CHỈ tự viết `BeginTransaction()`/`CommitAsync()`/`RollbackAsync()` thủ công khi **bắt buộc phải gọi `SaveChangesAsync()` nhiều lần** trong cùng 1 use case (ví dụ cần lấy Id auto-increment giữa chừng).
4. Việc phụ không bắt buộc thành công theo nghiệp vụ chính (gửi email, ghi log ngoài DB...) → tách riêng, tự `try-catch`, KHÔNG gộp vào transaction chính.
5. Dùng `Async` bắt buộc cho mọi thao tác chạm DB thật (`SaveChangesAsync`, `ToListAsync`, `FirstOrDefaultAsync`...). `Add()` không bắt buộc phải là `AddAsync()` vì không chạm DB ngay (dùng `Add()` thường cũng được).

```csharp
// Application/Events/EventService.cs — MẪU CHUẨN
public async Task<EventDto> CreateEventAsync(CreateEventDto input, CancellationToken ct)
{
    var ev = Event.Create(input.Title, input.StartDate);   // áp luật nghiệp vụ trong Domain
    await _eventRepository.AddAsync(ev, ct);                // chỉ đánh dấu
    await _unitOfWork.SaveChangesAsync(ct);                  // ghi thật, 1 LẦN DUY NHẤT

    return new EventDto { Id = ev.Id, Title = ev.Title, StartDate = ev.StartDate };
}
```

---

## 4. QUY TẮC EXCEPTION

| Loại                                      | Định nghĩa ở                 | Ném khi                                            |
| ----------------------------------------- | ---------------------------- | -------------------------------------------------- |
| `DomainException` (và class con)          | `Domain/Exceptions/`         | Vi phạm luật nghiệp vụ (ví dụ giá ≤ 0)             |
| `NotFoundException`, `ForbiddenException` | `Application/Exceptions/`    | Use case không thể tiếp tục do thiếu dữ liệu/quyền |
| `InfrastructureException`                 | `Infrastructure/Exceptions/` | Lỗi kỹ thuật khi thao tác DB/API ngoài             |

Tất cả exception bay tự do lên `WebApi`, bị bắt DUY NHẤT bởi 1 Middleware tập trung, map ra đúng HTTP status code (400/404/403/502/500).

---

## 5. QUY TẮC AUTH

- Dùng `[Authorize]`, `[Authorize(Roles = "Admin")]` thuần ASP.NET Core trên Controller.
- KHÔNG dùng MediatR trừ khi được yêu cầu rõ ràng thêm sau.
- Đọc `User.IsInRole(...)` CHỈ ở Controller, không đẩy khái niệm role xuống Application/Infrastructure.

---

## 6. CẤU TRÚC THƯ MỤC ĐẦY ĐỦ ĐỂ SCAFFOLD

```
src/
├── MyApp.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Exceptions/
│   └── MyApp.Domain.csproj
│
├── MyApp.Application/
│   ├── Common/
│   │   └── Interfaces/         # IEventRepository, IUnitOfWork...
│   ├── Exceptions/              # NotFoundException, ForbiddenException
│   ├── DTOs/                    # TẤT CẢ Request + Response, theo module
│   │   └── Events/
│   ├── Events/                  # Service theo module nghiệp vụ
│   │   └── EventService.cs
│   ├── DependencyInjection.cs
│   └── MyApp.Application.csproj
│
├── MyApp.Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/
│   │   ├── Repositories/
│   │   └── UnitOfWork.cs
│   ├── Exceptions/
│   ├── DependencyInjection.cs
│   └── MyApp.Infrastructure.csproj
│
└── MyApp.WebApi/
    ├── Controllers/
    ├── Middlewares/
    │   └── ExceptionHandlingMiddleware.cs
    ├── Program.cs
    ├── appsettings.json
    └── MyApp.WebApi.csproj
```

---

## 7. CHECKLIST — AGENT PHẢI TỰ KIỂM TRA SAU KHI SINH CODE

Trước khi coi 1 module là hoàn thành, agent phải tự rà theo checklist sau và sửa nếu sai:

- [ ] Không có file nào tên `*Request.cs` hoặc `*Response.cs` nằm trong `WebApi/` — nếu có, phải chuyển thành DTO trong `Application/DTOs/`.
- [ ] Mọi Controller action dùng thẳng DTO từ `Application.DTOs`, không có bước map DTO → Response riêng.
- [ ] `Domain` project không có bất kỳ `using` nào trỏ tới `Microsoft.AspNetCore.*` hoặc `Microsoft.EntityFrameworkCore` (ngoại trừ có thể cần `System.ComponentModel.DataAnnotations` nếu Entity cũng validate, nhưng ưu tiên để validate hình thức ở DTO).
- [ ] `Application` project không có `using Microsoft.AspNetCore.*` (không dùng `[FromBody]`, `[FromQuery]`, `IFormFile`...).
- [ ] Repository (Infrastructure) chỉ có method nhận/trả `Domain Entity`, KHÔNG có method nào nhận/trả DTO.
- [ ] Không có Repository method nào tự gọi `SaveChanges()`/`SaveChangesAsync()` bên trong — chỉ có `Add`/`Update`/`Remove`.
- [ ] Application Service gọi `SaveChangesAsync()` đúng 1 lần cho mỗi use case, trừ khi có comment giải thích rõ lý do cần nhiều lần + có `BeginTransaction` bọc ngoài.
- [ ] Không có Controller nào `inject` thẳng class từ `Infrastructure` (chỉ `Program.cs` được phép reference Infrastructure để đăng ký DI).
- [ ] Mọi method chạm DB (`ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`...) đều dùng bản `Async` + có `CancellationToken`.
- [ ] Có ít nhất 1 Middleware xử lý exception tập trung ở WebApi, không có `try-catch` xử lý HTTP status rải rác trong Controller.
- [ ] Auth dùng `[Authorize]` thuần ASP.NET Core, không có code MediatR nào xuất hiện trừ khi được yêu cầu.
- [ ] Nếu có phân biệt output theo role (Admin/Student...), DTO output phải tách bằng kế thừa (`AdminXxxDto : XxxDto`) ngay từ Application, không lộ field nhạy cảm ra DTO dùng chung.

---

## 8. KẾ HOẠCH DỰNG TOÀN PHẦN (thứ tự thực hiện đề xuất cho agent)

1. Tạo solution + 4 project theo cấu trúc mục 6, thiết lập đúng chiều reference ở mục 0.
2. Viết Domain: Entity cơ bản (`Order`, `Event`...) kèm method nghiệp vụ tự validate (`Create()`, `Update()`) và `DomainException`.
3. Viết Application: interface (`IXxxRepository`, `IUnitOfWork`), DTO (input + output, đặt đúng theo mục 1), Service gọi Domain + Repository, `DependencyInjection.cs`.
4. Viết Infrastructure: `AppDbContext`, `Configurations` (Fluent API), Repository implement đúng interface (chỉ nhận/trả Domain Entity), `UnitOfWork`, `DependencyInjection.cs`.
5. Viết WebApi: Controller mỏng (dùng thẳng DTO), Middleware exception, cấu hình Auth (`AddAuthentication`, `AddAuthorization`), `Program.cs` gọi `AddApplication()` + `AddInfrastructure()`.
6. Chạy lại toàn bộ Checklist ở mục 7, sửa mọi vi phạm trước khi báo hoàn thành.
7. (Tuỳ chọn, làm sau khi phần trên ổn định) Viết Unit Test cho Domain (test `Validate()`, business rule) và Application (test Service với Repository giả lập/mock).
