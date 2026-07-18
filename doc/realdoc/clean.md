```md
myproject/
│
├── myproject.sln # file solution tổng của dự án
│
└── src/ # thư mục chứa toàn bộ mã nguồn
│
├── myproject.domain/ # [project 1] tầng lõi (core domain)
│ ├── entities/ # các thực thể ứng dụng (bảng database)
│ │ ├── baseentity.cs # class chứa các thuộc tính chung (id, createdat...)
│ │ └── user.cs
│ ├── enums/ # các định nghĩa danh mục (enum)
│ │ └── userrole.cs
│ ├── exceptions/ # các custom exception xử lý logic nghiệp vụ
│ │ └── domainexception.cs
│ └── interfaces/ # định nghĩa các quy ước (contract) với hạ tầng
│ └── iuserrepository.cs
│ └── myproject.domain.csproj
│
├── myproject.application/ # [project 2] tầng xử lý logic nghiệp vụ (business logic)
│ ├── dtos/ # các đối tượng trung chuyển dữ liệu (data transfer objects)
│ │ ├── requests/ # dữ liệu client gửi lên
│ │ │ └── createuserrequest.cs
│ │ └── responses/ # dữ liệu hệ thống trả về
│ │ └── userdto.cs
│ ├── interfaces/ # các interface dịch vụ cung cấp cho api gọi
│ │ └── iuserservice.cs
│ ├── services/ # triển khai thực tế của các dịch vụ nghiệp vụ
│ │ └── userservice.cs
│ ├── mappings/ # cấu hình ánh xạ giữa entity và dto (automapper/mapperly)
│ │ └── mappingprofile.cs
│ ├── validators/ # kiểm tra tính hợp lệ dữ liệu đầu vào (fluentvalidation)
│ │ └── createuservalidator.cs
│ ├── dependencyinjection.cs # <-- file cấu hình/đăng ký di riêng cho tầng application
│ └── myproject.application.csproj
│
├── myproject.infrastructure/ # [project 3] tầng hạ tầng công nghệ (database, mail, jwt...)
│ ├── data/ # cấu hình kết nối cơ sở dữ liệu (ef core)
│ │ ├── applicationdbcontext.cs # class dbcontext chính quản lý các bảng
│ │ ├── configurations/ # cấu hình fluent api cho từng bảng cụ thể
│ │ │ └── userconfiguration.cs
│ │ └── migrations/ # thư mục tự động sinh ra khi thực hiện migration db
│ ├── repositories/ # triển khai thực tế các interface kết nối db từ tầng domain
│ │ └── userrepository.cs
│ ├── services/ # triển khai các dịch vụ kỹ thuật liên quan đến bên thứ ba
│ │ ├── jwtprovider.cs
│ │ └── emailservice.cs
│ ├── dependencyinjection.cs # <-- file cấu hình/đăng ký di riêng cho tầng infrastructure
│ └── myproject.infrastructure.csproj
│
└── myproject.presentation/ # [project 4] tầng api / hiển thị (nằm trong thư mục src)
├── controllers/ # nơi tiếp nhận các request http từ client
│ ├── baseapicontroller.cs # controller chung để chia sẻ cấu hình định tuyến (routing)
│ └── userscontroller.cs
├── middlewares/ # các lớp lọc request/response chung (ví dụ: global exception)
│ └── globalexceptionmiddleware.cs
├── appsettings.json # file cấu hình hệ thống (connection string, khóa bí mật jwt...)
├── appsettings.development.json # cấu hình riêng cho môi trường làm việc code (local)
├── program.cs # file khởi chạy ứng dụng & điểm kết nối di của tất cả các tầng
└── myproject.presentation.csproj
```

```md
[myproject.presentation] ────> [myproject.application] ────> [myproject.domain]
│ ▲
└───────────────────> [myproject.infrastructure] ────────────┘
```
