---
name: create-api
description: "Use when user asks to create a new API endpoint. Follows Clean Architecture (.NET 8)."
---

# Create API Endpoint

Base: c:\Users\phamq\OneDrive\Desktop\New folder (4)\BE-SEAL-HACKATHON

## Checklist

- [ ] Step 0: Check entities and enums
- [ ] Step 1: Determine role and controller
- [ ] Step 2: Check/create controller
- [ ] Step 3: Check/create Repository
- [ ] Step 4: Check/create Repository interface
- [ ] Step 5: Check/create Application Service
- [ ] Step 6: Write use case logic
- [ ] Step 7: Handle enum fields
- [ ] Step 8: Validate error messages
- [ ] Step 9: Register DI

---

## Step 0: Check Entities and Enums

Find entity in Hackathon.Domain/Entities/, enums in Hackathon.Domain/Enums/. Check properties, relationships, HasConversion in AppDbContext.OnModelCreating. Note foreign keys.

## Step 1: Role and Controller

Mapping: Admin->AdminController/AdminPolicy, Staff->StaffController/StaffPolicy, Lecturer->LecturerController/LecturerPolicy, Student->StudentController/StudentPolicy, Public->resource named/AllowAnonymous.

REST: GET/POST/PUT/DELETE /api/{role}/{resource}

## Step 2: Controller

Path: Hackathon.Presentation/Controllers/. Create with [Route("api/{role}")], [ApiController], inject I{Entity}Service, ApiResponseFactory.

## Step 3: Repository

Path: Hackathon.Infrastructure/Repositories/. Generic IRepository<T> in Application with CRUD. Implement with EF Core DbSet<T>. Custom only for complex queries.

## Step 4: Interface

Generic IRepository<T> covers basic. Custom goes in Application/Common/Interfaces/.

## Step 5: Application Service

Path: Hackathon.Application/Services/{Entity}/. Create Request.cs, Response.cs, Service.cs + interface in Application/Common/Interfaces/.

## Step 6: Logic

Application: validate, call interface, business rules, map, return. No EF Core. Infrastructure: queries, external services, SaveChangesAsync.

## Step 7: Handle Enum Fields

Enum flow: FE sends string -> JsonStringEnumConverter parses to enum in Controller -> Application uses enum for logic -> Response serializes enum back to string for FE.

- Body enum in Request DTO: declare property as domain enum type (e.g. EventStatusEnum Status). JsonStringEnumConverter auto-parses FE string -> enum and enum -> string. Invalid value -> JsonException -> GlobalExceptionHandlerMiddleware -> BadRequestException(ErrorMessage.Common.InvalidRequestData).
- Query/route string enum: declare as string in Request. In Service call EnumParser.ParseOrThrow<TEnum>(value, "FieldName") -> BadRequestException on invalid.
- ERROR MESSAGES: check ErrorMessage.cs. If missing, add new constant in appropriate category (e.g. ErrorMessage.Enum or ErrorMessage.{Entity}). Call via ErrorMessage.{Category}.{Name} when throwing.

## Step 8: Errors

Constants in ErrorMessage.cs. Format: Capitalize Each Word. Call via ErrorMessage.{Category}.{Name}.

## Step 9: DI

Infrastructure DI: repos, services, options. Application DI: AddScoped. Program.cs: verify AddApplication+AddInfrastructure and JsonStringEnumConverter.
