namespace Hackathon.Application.Services.Base.User;

public interface IUserProfileService
{
    Task<GetMyProfileResponse> GetMyProfile();
    Task UpdateProfile(UpdateProfileRequest request);
}
