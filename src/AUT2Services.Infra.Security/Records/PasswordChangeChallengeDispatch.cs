using AUT2Services.Domain.Core.Models;

namespace AUT2Services.Infra.Security.Records;

public sealed record PasswordChangeChallengeDispatch(
    string Code,
    string CodeHash,
    EmailDataModel EmailMessage);
