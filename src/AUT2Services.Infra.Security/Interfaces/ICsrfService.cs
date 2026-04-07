using Microsoft.AspNetCore.Http;

namespace AUT2Services.Infra.Security.Interfaces;

public interface ICsrfService
{
    void EnsureTokenCookie(HttpResponse response, string? existingToken = null);
    bool IsRequestValid(HttpRequest request);
}
