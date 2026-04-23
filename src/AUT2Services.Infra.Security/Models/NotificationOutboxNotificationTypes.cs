namespace AUT2Services.Infra.Security.Models;

public static class NotificationOutboxNotificationTypes
{
    public const string EmailConfirmation = "EMAIL_CONFIRMATION";
    public const string ZendeskForeignUserRegistration = "ZENDESK_FOREIGN_USER_REGISTRATION";
    public const string PasswordChangeVerificationCode = "PASSWORD_CHANGE_VERIFICATION_CODE";
}
