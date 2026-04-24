namespace AUT2Services.Infra.Security.Enumerations;

public static class EnumRefreshTokenRevocationReasons
{
    public const string Logout = "Logout";
    public const string Rotated = "Rotated";
    public const string ReuseDetected = "ReuseDetected";
    public const string SecondLogin = "SecondLogin";
    public const string PasswordRecovery = "PasswordRecovery";
}
