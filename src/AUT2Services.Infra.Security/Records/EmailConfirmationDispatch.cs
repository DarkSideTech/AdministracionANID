namespace AUT2Services.Infra.Security.Records;

public sealed record EmailConfirmationDispatch(
    string Email,
    string EncodedUserId,
    string EncodedToken,
    string ValidationToken,
    string AutoConfirmationUrl,
    string ManualConfirmationUrl
);
