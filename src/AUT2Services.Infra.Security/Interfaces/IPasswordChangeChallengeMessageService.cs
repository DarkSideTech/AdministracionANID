using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Records;

namespace AUT2Services.Infra.Security.Interfaces;

public interface IPasswordChangeChallengeMessageService
{
    PasswordChangeChallengeDispatch CreateDispatch(Usuario usuario, DateTimeOffset expiresAtUtc);
    PasswordChangeChallengeDispatch CreateRecoveryDispatch(Usuario usuario, DateTimeOffset expiresAtUtc);
    bool IsCodeMatch(string userId, string code, string expectedHash);
}
