using Microsoft.AspNetCore.Http;

namespace Hackathon.Application.Services.Base.User;

public interface IUserProfileService
{
    Task<GetMyProfileResponse> GetMyProfile();
    Task UpdateProfile(UpdateProfileRequest request);
    Task<string> UpdateAvatar(IFormFile file);
}
