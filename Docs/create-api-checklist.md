# Create API Checklist — Clean Architecture .NET 8

## Form Đối Chiếu Khi Tạo API

### 1. Xác Định Endpoint

| Câu hỏi | Trả lời |
|---------|---------|
| Role gì? | Admin / Staff / Lecturer / Student / Public |
| Resource gì? | Events / Users / Teams / ... |
| HTTP method? | GET / POST / PUT / PATCH / DELETE |
| URL pattern? | `/api/{role}/{resource}` |

### 2. Controller (Presentation Layer)

- [ ] File {Role}Controller.cs da ton tai?
- [ ] Route dung: [Route("api/{role}")]
- [ ] Auth dung policy
- [ ] Su dung [ApiController], ke thua ControllerBase
- [ ] Inject service interface
- [ ] Tra ve IActionResult voi ApiResponseFactory.*
- [ ] Truyen HttpContext.TraceIdentifier

### 3. Application Service (Use Case Layer)

- [ ] Interface I{Entity}Service.cs trong Application/Common/Interfaces/
- [ ] Implementation Service.cs trong Application/Services/{Entity}/
- [ ] Request DTO (neu can input)
- [ ] Response DTO (dau ra chuan)
- [ ] Validation logic (business rules)
- [ ] Goi repository interface (khong goi DbContext truc tiep)

### 4. Repository (Data Layer)

- [ ] Dung IRepository<T> generic hoac custom?
- [ ] Neu custom: interface o Application, implementation o Infrastructure
- [ ] Query du Include/Where/Select?
- [ ] SaveChangesAsync duoc goi sau mutation
- [ ] IUnitOfWork duoc inject dung

### 5. Error Messages

- [ ] Tat ca throw dung ErrorMessage.* constants
- [ ] Format **Capitalize Each Word**
- [ ] Da kiem tra ErrorMessage.cs truoc khi tao moi
- [ ] Category class phu hop (Auth, Database, Media, Pagination,...)

### 6. Dependency Injection

- [ ] Interface + implementation registered trong Infrastructure/DependencyInjection.cs
- [ ] Application service registered trong Application/DependencyInjection.cs
- [ ] Options config: services.Configure<T>(configuration.GetSection("T"))
- [ ] Program.cs goi AddApplication() + AddInfrastructure()

### 7. Clean Architecture Rules

| Layer | Duoc phep | Khong duoc phep |
|-------|-----------|----------------|
| Application | Dung Domain, Interface tu Application | Dung Infrastructure, DbContext, EF Core |
| Infrastructure | Dung Application, Domain | Bi Application goi truc tiep |
| Presentation | Dung Application | Dung Infrastructure truc tiep |

## Quy Tac Loi

| Exception | HTTP | Khi nao dung |
|-----------|------|-------------|
| BadRequestException(msg) | 400 | Input khong hop le |
| UnauthorizedException(msg) | 401 | Token het han/sai |
| ForbiddenException(msg) | 403 | Khong co quyen |
| NotFoundException(msg) | 404 | Khong tim thay entity |
| ConflictException(msg) | 409 | Trung lap, conflict |
| TooManyRequestException(msg) | 429 | Rate limit |
| ServerException(msg) | 500 | Loi internal |
| FileUploadFailedException(msg) | 500 | Upload file that bai |
| ServiceUnavailableException(msg) | 503 | Service ngoai down |

## Form Loi Chuan

Error message format: **Capitalize Each Word**

Dung: "User Not Found", "Save Changes Failed", "Invalid Email Or Password"
Sai: "user_not_found", "USER_NOT_FOUND", "SaveChangesFailed", "Invalid email or password"
