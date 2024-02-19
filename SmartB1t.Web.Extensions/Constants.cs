namespace SmartB1t.Web.Extensions;

public static class TempDataAlertModelState
{
    public static readonly string Created = nameof(Created);
    public static readonly string Updated = nameof(Updated);
    public static readonly string Removed = nameof(Removed);
}

public static class TempDataAlertConstants
{
    public static readonly string UserDeactivated = nameof(UserDeactivated);
    public static readonly string UserDeactivatedEmail = nameof(UserDeactivatedEmail);
    public static readonly string UserLoggedOut = nameof(UserLoggedOut);
    public static readonly string EmailChanged = nameof(EmailChanged);
    public static readonly string PasswordChanged = nameof(PasswordChanged);
}
