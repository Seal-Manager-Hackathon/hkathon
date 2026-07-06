namespace Hackathon.Application.Common;

public static class SuccessMessage
{
    public static class Auth
    {
        public const string LoginSuccessful = "Login Successful";
        public const string RegisterSuccessful = "Registration Successful";
        public const string LogoutSuccessful = "Logout Successful";
        public const string EmailVerified = "Email Verified Successfully";
        public const string EmailVerificationSent = "Email Verification Sent";
        public const string PasswordChanged = "Password Changed Successfully";
        public const string PasswordReset = "Password Reset Successfully";
        public const string ForgotPasswordRequestAccepted = "Forgot Password Request Accepted";
        public const string TokenRefreshed = "Token Refreshed Successfully";
    }

    public static class User
    {
        public const string ProfileUpdated = "Profile Updated Successfully";
        public const string AvatarUpdated = "Avatar Updated Successfully";
        public const string ProfileFetched = "Profile Fetched Successfully";
    }

    public static class Media
    {
        public const string FileUploaded = "File Uploaded Successfully";
        public const string FileDeleted = "File Deleted Successfully";
    }

    public static class Common
    {
        public const string OperationSuccessful = "Operation Successful";
        public const string Created = "Created Successfully";
        public const string Updated = "Updated Successfully";
        public const string Deleted = "Deleted Successfully";
        public const string Fetched = "Fetched Successfully";
    }

    public static class Team
    {
        public const string Created = "Team Created Successfully";
        public const string Updated = "Team Updated Successfully";
        public const string Deleted = "Team Deleted Successfully";
        public const string MemberAdded = "Member Added Successfully";
        public const string MemberRemoved = "Member Removed Successfully";
        public const string Joined = "Joined Team Successfully";
        public const string Left = "Left Team Successfully";
        public const string InvitationSent = "Invitation Sent Successfully";
        public const string InvitationAccepted = "Invitation Accepted Successfully";
        public const string InvitationDeclined = "Invitation Declined Successfully";
    }

    public static class Event
    {
        public const string Created = "Event Created Successfully";
        public const string Updated = "Event Updated Successfully";
        public const string Deleted = "Event Deleted Successfully";
        public const string Registered = "Registered For Event Successfully";
        public const string Unregistered = "Unregistered From Event Successfully";
    }

    public static class Submission
    {
        public const string Created = "Submission Created Successfully";
        public const string Updated = "Submission Updated Successfully";
        public const string Deleted = "Submission Deleted Successfully";
        public const string Submitted = "Submitted Successfully";
    }

    public static class Report
    {
        public const string Created = "Report Created Successfully";
    }

    public static class Admin
    {
        public const string UserBanned = "User Banned Successfully";
        public const string UserUnbanned = "User Unbanned Successfully";
        public const string RoleUpdated = "User Role Updated Successfully";
    }
}
