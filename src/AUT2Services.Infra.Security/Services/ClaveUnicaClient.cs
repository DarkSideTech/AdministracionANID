using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AUT2Services.Infra.Security.Services;

public sealed class ClaveUnicaClient(
    IHttpClientFactory httpClientFactory,
    IOptions<ClaveUnicaOptions> options,
    ILogger<ClaveUnicaClient> logger) : IClaveUnicaClient
{
    public const string HttpClientName = "ClaveUnica";

    private readonly ClaveUnicaOptions options = options.Value;
    private readonly ILogger<ClaveUnicaClient> logger = logger;

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(options.ClientId)
        && !string.IsNullOrWhiteSpace(options.ClientSecret)
        && !string.IsNullOrWhiteSpace(options.RedirectUri)
        && !string.IsNullOrWhiteSpace(options.TokenUrl)
        && !string.IsNullOrWhiteSpace(options.UserInfoUrl);

    public async Task<string> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(HttpClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, options.TokenUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = options.ClientId,
                ["client_secret"] = options.ClientSecret,
                ["redirect_uri"] = options.RedirectUri,
                ["grant_type"] = "authorization_code",
                ["code"] = code
            })
        };

        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Clave Unica rechazo el intercambio del codigo con estado HTTP {StatusCode}.", (int)response.StatusCode);
            throw new InvalidOperationException("Clave Unica rechazo el codigo de autorizacion.");
        }

        var payload = JsonSerializer.Deserialize<ClaveUnicaTokenResponse>(
            await response.Content.ReadAsStringAsync(cancellationToken),
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            throw new InvalidOperationException("Clave Unica no devolvio un access token valido.");
        }

        return payload.AccessToken;
    }

    public async Task<ClaveUnicaUserInfoResponse> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken)
    {
        using var client = httpClientFactory.CreateClient(HttpClientName);
        using var request = new HttpRequestMessage(HttpMethod.Post, options.UserInfoUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await client.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Clave Unica rechazo la consulta de datos de usuario con estado HTTP {StatusCode}.", (int)response.StatusCode);
            throw new InvalidOperationException("Clave Unica no permitio consultar los datos del usuario.");
        }

        var payload = JsonSerializer.Deserialize<ClaveUnicaUserInfoResponse>(
            await response.Content.ReadAsStringAsync(cancellationToken),
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        return payload ?? throw new InvalidOperationException("Clave Unica no devolvio datos de usuario validos.");
    }
}
