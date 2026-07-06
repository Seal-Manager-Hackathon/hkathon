---
name: check-api
description: "Use to validate an existing API endpoint against Clean Architecture standards."
---

# Check API Endpoint

## Controller

- Route matches role? [Route("api/{role}")]
- Auth correct? [Authorize] or [AllowAnonymous]
- Inherits ControllerBase with [ApiController]?
- Correct binding attributes?
- Returns IActionResult with ApiResponseFactory?

## Application Service

- Interface exists in Application/Common/Interfaces/?
- Implementation in Application/Services/{Entity}/Service.cs?
- Methods match?

## Data Access

- Queried via interface, not direct DbContext in Application?
- SaveChangesAsync after mutations?
- IUnitOfWork used?
- No Application references Infrastructure?

## Enum

- Body enums use domain enum type in Request DTO? JsonStringEnumConverter handles parse.
- Query/route enum strings parsed via EnumParser.ParseOrThrow?
- Invalid enum produces proper error? Error message exists or added in ErrorMessage.cs?
- JsonStringEnumConverter registered in Program.cs?

## Error Messages

- All throw uses ErrorMessage.* constants?
- No hardcoded strings?
- No UPPER_SNAKE or lowercase?
- Format: Capitalize Each Word?

## DI

- Service in Application/DependencyInjection.cs?
- Repo/service in Infrastructure/DependencyInjection.cs?
- Options registered if using IOptions?
- Controller injects interface?

## Boundaries

- Application references Domain only?
- Infrastructure implements Application interfaces?
- Presentation references Application only?
