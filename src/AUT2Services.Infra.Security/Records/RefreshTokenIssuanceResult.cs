using AUT2Services.Domain.Security.Entities;

namespace AUT2Services.Infra.Security.Records;

public sealed record RefreshTokenIssuanceResult(
    string PlainTextToken,
    RefreshToken RefreshToken
);
