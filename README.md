# SEAL Hackathon Management System

Hệ thống backend quản lý hackathon từ đầu đến cuối: quản lý tài khoản, đội nhóm, sự kiện, đăng ký tham gia, bốc thăm đề tài, nộp bài, chấm điểm, báo cáo/phúc khảo, phân vòng, bảng xếp hạng và thông báo.

Xây dựng bằng **.NET 8**, **Entity Framework Core** và **PostgreSQL**, theo **Clean Architecture**.

---

## Tính năng chính

Hệ thống xoay quanh **3 luồng nghiệp vụ chính**:

**1. Tài khoản & đội nhóm** — Người dùng đăng ký tài khoản, xác thực email, quản lý thông tin cá nhân. Sinh viên tạo đội, mời thành viên, chấp nhận/từ chối lời mời, chuyển quyền trưởng nhóm.

**2. Đăng ký tham gia cuộc thi** — Admin tạo sự kiện và thiết lập đầy đủ (vòng thi, tiêu chí chấm, track, đề tài, giải thưởng, phân công). Trưởng nhóm đăng ký đội tham gia, staff duyệt/từ chối/cấm đội.

**3. Thi đấu, chấm điểm & xếp hạng** — Các đội được đưa vào từng vòng thi, nộp bài và được giám khảo chấm theo tiêu chí. Điểm tự động tổng hợp theo 3 cấp (điểm vòng → điểm sự kiện → điểm mùa/năm), từ đó xếp hạng, nâng vòng và trao giải.

Ngoài ra còn có: quản lý track/đề tài kèm bốc thăm offline, phân công giảng viên làm mentor/judge, thông báo, báo cáo và phúc khảo điểm.

---

## Cài đặt & chạy

### 1. Cài .NET SDK 8.0

Tải và cài **.NET SDK 8.0** tại: <https://dotnet.microsoft.com/download/dotnet/8.0>

Kiểm tra đã cài thành công:

```bash
dotnet --version
```

### 2. Cài PostgreSQL

Cần một instance **PostgreSQL** đang chạy (tải tại <https://www.postgresql.org/download/>).

### 3. Tải mã nguồn

```bash
git clone https://github.com/Seal-Manager-Hackathon/hkathon.git
cd hkathon
```

### 4. Cấu hình

Repo có sẵn file mẫu `appsettings.example.json`. Tạo file cấu hình thật bằng cách **bỏ chữ `.example`** (đổi tên):

```bash
cp Hackathon.Presentation/appsettings.example.json Hackathon.Presentation/appsettings.json
```

> Windows (PowerShell):
>
> ```powershell
> Copy-Item Hackathon.Presentation/appsettings.example.json Hackathon.Presentation/appsettings.json
> ```

Mở file `Hackathon.Presentation/appsettings.json` và điền các giá trị thật (quan trọng nhất là chuỗi kết nối PostgreSQL):

| Khóa                                  | Ý nghĩa                                       |
| ------------------------------------- | --------------------------------------------- |
| `ConnectionStrings:DefaultConnection` | Chuỗi kết nối PostgreSQL                      |
| `JwtOptions:SecretKey`                | Khóa bí mật ký JWT (tạo chuỗi ngẫu nhiên dài) |
| `CloudinaryOptions:*`                 | Tài khoản Cloudinary (upload ảnh/file)        |
| `MailOptions:*`                       | SMTP gửi mail (xác thực email, quên mật khẩu) |
| `SecurityOptions:Pepper`              | Chuỗi bí mật nối vào mật khẩu trước khi băm   |

**Hướng dẫn thiết lập các dịch vụ bên thứ ba:**

- **JWT** — chỉ cần điền `JwtOptions:SecretKey` bằng một chuỗi ngẫu nhiên dài (64+ ký tự). Có thể sinh nhanh bằng:

  ```bash
  openssl rand -base64 48
  ```

  `Issuer` và `Audience` để mặc định là được.

- **Cloudinary** (upload ảnh/file) — tạo tài khoản miễn phí tại <https://cloudinary.com>, vào **Dashboard** lấy 3 giá trị:
  - `CloudName` → `CloudinaryOptions:CloudName`
  - `API Key` → `CloudinaryOptions:ApiKey`
  - `API Secret` → `CloudinaryOptions:ApiSecret`

- **Gmail** (gửi mail xác thực / quên mật khẩu) — Gmail không cho dùng mật khẩu thường để gửi qua SMTP, phải tạo **App Password**:
  1. Bật xác minh 2 bước tại <https://myaccount.google.com/security>
  2. Vào **App passwords** → tạo một app password
  3. Điền vào `MailOptions`:
     - `Mail`: địa chỉ Gmail gửi
     - `Password`: app password vừa tạo
     - `Host`: `smtp.gmail.com`, `Port`: `587`

> ⚠️ **Ghi chú quan trọng:** Trong mã nguồn có một số đoạn validation đang được **comment lại để tiện test**. Khi đưa lên chạy thật (production), hãy **tháo các dòng comment** đó ra để bật lại đầy đủ ràng buộc nghiệp vụ (ví dụ kiểm tra thời gian nộp bài, thời gian duyệt đăng ký, thời gian của round... trong các file thuộc `Hackathon.Application/Services`).

### 5. Tạo database (chạy migration)

Cài công cụ EF (nếu chưa có):

```bash
dotnet tool install --global dotnet-ef
```

Chạy migration để tạo database và seed dữ liệu:

```bash
cd Hackathon.Presentation
dotnet ef database update
```

> Migration `Initial` đã có sẵn. Nếu muốn tự thêm migration mới: `dotnet ef migrations add <Tên> --project ../Hackathon.Infrastructure`. Muốn xóa migration vừa thêm nhầm: `dotnet ef migrations remove --project ../Hackathon.Infrastructure`.

### 6. Chạy

```bash
dotnet run
```

Mở Swagger UI tại: **http://localhost:5247/swagger**

> Cổng mặc định lấy từ `Properties/launchSettings.json` (`http://localhost:5247`). Nếu chạy không có launch profile, lấy cổng hiển thị trên console.

Để gọi các API cần quyền: bấm nút **Authorize** trên Swagger, dán JWT theo dạng `Bearer <token>`.

### Tài khoản seed (để test)

Sau khi chạy migration, hệ thống tự seed dữ liệu mẫu. Mật khẩu chung: **`string`**

| Email               | Vai trò                 |
| ------------------- | ----------------------- |
| `admin@seed.local`  | Admin                   |
| `staff@seed.local`  | Staff                   |
| `leader@seed.local` | Sinh viên (trưởng nhóm) |
| `judge@seed.local`  | Giảng viên (giám khảo)  |

---

## Công nghệ & kiến trúc

### Kiến trúc 4 tầng (Clean Architecture)

```
Hackathon.Presentation  →  Tầng HTTP: controller, middleware, Swagger, cấu hình JWT
Hackathon.Application   →  Tầng nghiệp vụ: service, DTO, validation, helper
Hackathon.Domain        →  Entity, enum, exception (không phụ thuộc tầng nào)
Hackathon.Infrastructure →  DbContext, repository, UnitOfWork, dịch vụ ngoài (DB, mail, cloud), seed
```

Chiều phụ thuộc: `Presentation → Application → Domain ← Infrastructure`. Tầng `Domain` không phụ thuộc bất kỳ tầng nào khác.

### Công nghệ chính

| Thành phần         | Công nghệ                     |
| ------------------ | ----------------------------- |
| Ngôn ngữ / runtime | C# / .NET 8                   |
| Database           | PostgreSQL (EF Core + Npgsql) |
| Xác thực           | JWT Bearer, BCrypt            |
| Validation         | FluentValidation              |
| API docs           | Swagger (Swashbuckle)         |
| Tác vụ nền         | Quartz                        |
| Gửi mail           | MailKit                       |
| Upload file        | Cloudinary                    |

### Vai trò người dùng

- **Admin** — toàn quyền: quản lý sự kiện, người dùng, phân công, báo cáo, bảng xếp hạng.
- **Staff** — vận hành sự kiện: duyệt đăng ký, phân công giảng viên, xử lý báo cáo/phúc khảo.
- **Student** — tạo/tham gia đội, đăng ký, nộp bài.
- **Lecturer** — được phân công làm **Mentor** (hỗ trợ, không chấm) hoặc **Judge** (chấm điểm) theo từng sự kiện/track.

---

## Cấu trúc thư mục

```
hkathon/
├── Hackathon.Presentation/      # Controller, Program.cs, middleware, Swagger/JWT
│   ├── Controllers/             #   Auth, Base, Admin, Staff, Lecturer, Judge, Mentor, Student
│   ├── appsettings.example.json #   File cấu hình mẫu (được commit)
├── Hackathon.Application/       # Service (theo vai trò), DTO, validation, helper
├── Hackathon.Domain/            # Entity, enum, exception
├── Hackathon.Infrastructure/    # DbContext, repository, UnitOfWork, dịch vụ ngoài, seed, job
└── Hackathon.sln
```
