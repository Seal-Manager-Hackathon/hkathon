# Update 08 — 11/07/2026

## 1. Sửa route Judge tracks
**Trước:** `GET /api/v1/judge/tracks` (ko cần eventId)  
**Sau:** `GET /api/v1/judge/events/{eventId}/tracks` (giống Mentor pattern)  
**File:** `Controllers/Judge/JudgeController.cs` + `Services/Judge/IJudgeService.cs` + `Services/Judge/Service.cs`

## 2. Sửa route StaffAssign (thêm `assign/`)
**Trước:** `[Route("api/v1/staff")]`  
**Sau:** `[Route("api/v1/staff/assign")]`  
**File:** `Controllers/Staff/StaffAssignController.cs`

## 3. Thêm endpoint Staff staff/available
**Trước:** Staff ko có API `GET events/{eventId}/staff/available`  
**Sau:** Thêm mới endpoint cho StaffAssignController  
**Files:** `Services/Staff/Assign/IAssignService.cs` (+ method), `Services/Staff/Assign/Service.cs` (+ impl), `Services/Staff/Assign/Response.cs` (+ DTOs), `Controllers/Staff/StaffAssignController.cs` (+ route)

## 4. Sửa route Award detail (bỏ eventId)
**Trước:** `GET /api/v1/admin/events/{eventId}/awards/{awardId}`  
**Sau:** `GET /api/v1/admin/awards/{awardId}`  
**File:** `Controllers/Admin/AdminAwardController.cs`

## 5. Đồng bộ chapter leaderboard Admin/Staff
**Trước:** Staff `GetChapterLeaderboard` lọc theo event assign → kết quả khác Admin  
**Sau:** Staff bỏ lọc assign, dùng thẳng helper (giống Admin)  
**File:** `Services/Staff/Leaderboard/Service.cs`

## 6. Sửa sắp xếp available staff, lecturers, assigned users
**Trước:** Sắp xếp theo `FirstName, LastName`  
**Sau:** Sắp xếp theo `Email` (đồng bộ giữa staff available, lecturer available, assigned)  
**Files:** `Repositories/UserRepository.cs` (GetAvailableUsersByRoleAsync), `Repositories/AssignEventRepository.cs` (GetAssignedUsersByEventAsync — cả 2 overload)

## 7. Sửa route Admin Track detail + update (bỏ eventId)
**Trước:** `GET /api/v1/admin/events/{eventId}/tracks/{trackId}` và `PATCH /api/v1/admin/events/{eventId}/tracks/{trackId}`  
**Sau:** `GET /api/v1/admin/tracks/{trackId}` và `PATCH /api/v1/admin/tracks/{trackId}` (bỏ eventId vì track đã biết event của nó)  
**Files:** `Controllers/Admin/AdminTrackController.cs`, `Services/Admin/Track/ITrackService.cs`, `Services/Admin/Track/Service.cs`, `documents/admin/track/get/admin.tracks.detail.md`
**Trước:** Sắp xếp theo `FirstName, LastName`  
**Sau:** Sắp xếp theo `Email` (đồng bộ giữa staff available, lecturer available, assigned)  
**Files:** `Repositories/UserRepository.cs` (GetAvailableUsersByRoleAsync), `Repositories/AssignEventRepository.cs` (GetAssignedUsersByEventAsync — cả 2 overload)
