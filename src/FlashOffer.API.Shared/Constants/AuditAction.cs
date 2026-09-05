namespace FlashOffer.API.Shared.Constants;

/// <summary>
/// Hằng số mô tả hành động cho audit log.
/// </summary>
public static class AuditAction
{
    // Entity actions
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";

    // Auth actions
    public const string Login = "Login";
    public const string Logout = "Logout";
    public const string Register = "Register";
    public const string RefreshToken = "RefreshToken";
    public const string ChangePassword = "ChangePassword";
    public const string ResetPassword = "ResetPassword";
}