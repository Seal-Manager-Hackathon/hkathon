Domain:

> BE-SEAL-HACKATHON.Domain csproj:
  - Microsoft.EntityFrameworkCore 8.0.11
  - Npgsql.EntityFrameworkCore.PostgreSQL 8.0.11

Application:

> BE-SEAL-HACKATHON.Application csproj:
  - BCrypt.Net-Next 4.0.3
  - FluentValidation 11.11.0
  - Microsoft.AspNetCore.Http 2.3.0
  - Microsoft.Extensions.Configuration.Binder 8.0.2
  - Quartz 3.13.1 (+ Extensions.Hosting 3.13.1)
  - System.IdentityModel.Tokens.Jwt 8.3.1
  -> ProjectRef: BE-SEAL-HACKATHON.Domain

Infrastructure:

> BE-SEAL-HACKATHON.Infrastructure csproj:
  - CloudinaryDotNet 1.26.2
  - MailKit 4.8.0
  - Microsoft.AspNetCore.Authentication.JwtBearer 8.0.11
  -> ProjectRef: BE-SEAL-HACKATHON.Application

Presentation:

> BE-SEAL-HACKATHON.Presentation csproj:
  - FluentValidation.AspNetCore 11.3.0
  - Microsoft.EntityFrameworkCore.Design 8.0.11
  - Swashbuckle.AspNetCore 6.9.0
  -> ProjectRef: BE-SEAL-HACKATHON.Infrastructure
