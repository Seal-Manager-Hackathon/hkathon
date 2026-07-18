# POST /api/v1/auth/refresh-token

> Refresh access token bằng refresh token. Token rotation: revoke token cũ, tạo token mới.

## Nghiệp vụ
- Nhận refresh token từ body
- Tìm refresh token trong DB theo hash
- Kiểm tra token còn active không: `RevokedAt == null && ExpiredAt > now`
- Revoke token cũ (set `RevokedAt = now`)
- Tạo refresh token mới (token rotation)
- Tạo access token mới
- Trả về cả access + refresh token mới

## Phân quyền
- **Public** — không cần Bearer token (dùng refresh token thay thế)

## Request
```json
{
  "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
}
```

| Field | Bắt buộc | Ràng buộc |
|-------|----------|-----------|
| refreshToken | ✅ | Refresh token từ login/verify-email |

## Response (200)
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "eyJhbGciOiJIUzI1NiIs..."
  },
  "message": "Token Refreshed Successfully",
  "status": 200,
  "traceId": "00-abc123..."
}
```

## Lỗi
| Status | message | Khi nào |
|--------|---------|---------|
| 401 | Invalid Refresh Token | Token ko tồn tại, đã bị thu hồi, hoặc hết hạn |

## Logic
1. Tìm refresh token trong DB theo hash
2. Check active: `RevokedAt == null && ExpiredAt > now`
3. Revoke token cũ (set `RevokedAt`)
4. Tạo refresh token mới + access token mới
5. Trả về cặp token mới
