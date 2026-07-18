# CAU TRUC DU AN - BE-SEAL-HACKATHON (Clean Architecture)

> Ngay: 04/07/2026
> Trang thai: Dang tai cau truc theo Clean Architecture
> Code cu van giu nguyen tai `Hackathon.Api/`, `Hackathon.Service/`, `Hackathon.Repository/`

```
BE-SEAL-HACKATHON/
│
├── BE-SEAL-HACKATHON.sln                    # Solution tong
├── global.json
├── CLAUDE.md, AGENTS.md
│
├── doc/                                     # Tai lieu du an
│   ├── contexxt.md                          # Business rules chi tiet
│   ├── NewdocContext.md                     # Mo ta event flow
│   ├── PAGE_API.txt                         # FE -> API mapping
│   ├── clean.md                             # Clean Architecture template
│   └── update-structure.md                  # File nay
│
├══════════════════════════════════════════════════════════════════
║  [NEW] CLEAN ARCHITECTURE (DA CO CAU TRUC, CHO FILL CODE)
║
├── BE-SEAL-HACKATHON.Domain/                # Layer 1: Core Domain = Hackathon.Repository (Entities + Enums)
│   ├── BE-SEAL-HACKATHON.Domain.csproj
│   ├── Entities/                            # (27 entities)
│   │   ├── BaseEntity.cs                    # Id, IsDisable, CreatedAt, UpdatedAt
│   │   ├── IAuditableEntity.cs
│   │   ├── Events.cs / Rounds.cs / RoundDetails.cs
│   │   ├── Teams.cs / TeamDetails.cs
│   │   ├── RegisterTeams.cs
│   │   ├── Submissions.cs / Scores.cs / ScoreItems.cs
│   │   ├── Tracks.cs / Topics.cs
│   │   ├── Awards.cs / LeaderBoards.cs / LeaderBoardDetails.cs
│   │   ├── AssignEvents.cs / AssignTracks.cs
│   │   ├── CriteriaTemplates.cs / CriteriaItems.cs
│   │   ├── Invitations.cs / Notifications.cs / MentorNotifications.cs
│   │   ├── Users.cs / RefreshTokens.cs / ResetPasswords.cs
│   │   ├── EmailVerifications.cs / Reports.cs / EventRoles.cs
│   │   └── (con lai...)
│   ├── Enums/                               # (14 enums)
│   │   ├── RoleEnum.cs                      # Admin, Staff, Student, Lecturer
│   │   ├── EventRoleEnum.cs                 # Mentor, Judge, Staff
│   │   ├── EventStatusEnum.cs               # Draft, Published, Closed
│   │   ├── SeasonEnum.cs
│   │   ├── RegisterTeamStatusEnum.cs
│   │   ├── SubmissionStatusEnum.cs
│   │   ├── ScoresStatusEnum.cs
│   │   ├── InvitationStatusEnum.cs
│   │   ├── UserStatusEnum.cs / TeamDetailStatusEnum.cs
│   │   ├── ReportStatusEnum.cs
│   │   ├── NotificationStatusEnum.cs / NotificationTargetTypeEnum.cs
│   │   └── EmailVerificationStatusEnum.cs
│   ├── Exceptions/                          # (4 files)
│   │   ├── AppException.cs
│   │   ├── AuthExceptions.cs
│   │   ├── CommonExceptions.cs
│   │   └── LeaderBoardExceptions.cs
│   └── Interfaces/                          # Domain repo interfaces
│       └── IRepository.cs
│
├── BE-SEAL-HACKATHON.Application/           # Layer 2: Business Logic = Hackathon.Service (Services)
│   ├── BE-SEAL-HACKATHON.Application.csproj
│   ├── Models/                              # ApiResponse, PaginationRequest...
│   ├── Mappings/                            # AutoMapper profiles
│   ├── Validators/                          # FluentValidation validators
│   └── Services/                            # Service modules (23 folders)
│       ├── Events/                          #   Moi folder co:
│       │   ├── IService.cs                  #     - Interface service
│       │   ├── Service.cs                   #     - Implement business logic
│       │   ├── Request.cs                   #     - Request DTOs
│       │   └── Response.cs                  #     - Response DTOs
│       ├── Admin/
│       ├── Rounds/
│       ├── RegisterTeams/
│       ├── Staff/
│       ├── Tracks/
│       ├── Criticals/
│       ├── AssignEvents/
│       ├── AssignTracks/
│       ├── Submissions/
│       ├── Invitations/
│       ├── Judges/
│       ├── Teams/
│       ├── Mentors/
│       ├── Lecturers/
│       ├── LeaderBoards/
│       ├── Notifications/
│       ├── Topics/
│       ├── Roles/
│       ├── Systems/
│       ├── Auths/
│       ├── Users/
│       └── BackgroundJob/
│
├── BE-SEAL-HACKATHON.Infrastructure/        # Layer 3: Infrastructure [RONG]
│   ├── BE-SEAL-HACKATHON.Infrastructure.csproj
│   ├── Data/
│   │   ├── Configurations/                  # EF Fluent API configs
│   │   ├── Migrations/                      # EF Core migrations
│   │   └── Seed/                            # Seed data
│   ├── Repositories/
│   └── Services/                            # JWT, Mail, Cloudinary...
│
├── BE-SEAL-HACKATHON.Presentation/          # Layer 4: API [RONG]
│   ├── BE-SEAL-HACKATHON.Presentation.csproj
│   ├── Controllers/                         # API endpoints
│   ├── Extensions/                          # JwtExtension, Swagger, RateLimit...
│   ├── Middleware/                          # GlobalExceptionHandler...
│   ├── Filters/
│   ├── Localization/
│   ├── Resources/
│   └── Properties/                          # launchSettings
│
├══════════════════════════════════════════════════════════════════
║  [OLD] CODE HIEN TAI (giu nguyen, build van chay)
║
├── Hackathon.Api/                           # Presentation (OLD)
│   ├── Program.cs
│   ├── Controllers/                         # 20 controllers
│   ├── Extention/                           # Extensions
│   ├── Middleware/
│   ├── Filters/
│   ├── Localization/
│   ├── Resources/
│   ├── Docs/ApiDocs/                        # API documentation
│   └── appsettings.json
│
├── Hackathon.Service/                       # Business Logic (OLD)
│   ├── Events/ / Admin/ / Rounds/ / RegisterTeams/ ...
│   ├── Auths/ / Users/ / Teams/ / Judges/ / Mentors/ ...
│   ├── JwtServices/ / MailServices/ / CloudinaryService/ ...
│   ├── BackgroundJobService/
│   ├── Models/ / Validations/ / Exceptions/ / Localization/
│   └── (Moi module: IService.cs, Service.cs, Request.cs, Response.cs)
│
└── Hackathon.Repository/                    # Data Layer (OLD)
    ├── AppDbContext.cs
    ├── Abtraction/                          # BaseEntity, IAuditableEntity
    ├── Entity/                              # 27 entity classes
    ├── Enum/                                # 14 enum files
    ├── Migrations/                          # EF Core migrations
    └── Seed/                                # 19 seed data files
```

---
## Layer Dependencies

```
Presentation (Web API)
  -> Infrastructure
      -> Application
          -> Domain

Domain      -> khong phu thuoc gi
Application -> Domain
Infrastructure -> Application (qua Domain)
Presentation -> Infrastructure + Application
```

## Structure Trong Application/Services/{Entity}/

```
Services/Events/
  +-- IService.cs        # Interface (cac method CRUD + business)
  +-- Service.cs         # Implement logic
  +-- Request.cs         # Request DTOs (client gui len)
  +-- Response.cs        # Response DTOs (he thong tra ve)
```

## Status Legend
| Ky hieu | Y nghia |
|---------|---------|
| [OK]    | Da co code, build OK |
| [OLD]   | Con code cu, chua migrate |
| [RONG]  | Folder rong, cho fill sau |
