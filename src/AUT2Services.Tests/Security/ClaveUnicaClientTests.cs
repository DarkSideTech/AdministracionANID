using AUT2Services.Infra.Security.Services;
using AUT2Services.Infra.Security.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace AUT2Services.Tests.Security;

[TestClass]
public class ClaveUnicaClientTests
{
    [TestMethod]
    public async Task ExchangeCodeAsync_UsesServerConfigurationAndDoesNotSendOAuthStateToTokenEndpoint()
    {
        var handler = new CapturingHttpMessageHandler("{\"access_token\":\"sandbox-access-token\"}");
        var client = new ClaveUnicaClient(
            new StubHttpClientFactory(handler),
            Options.Create(new ClaveUnicaOptions
            {
                ClientId = "sandbox-client-id",
                ClientSecret = "sandbox-test-secret",
                RedirectUri = "https://mi.desa.anid.gob.cl/authentication/callback-clave-unica",
                TokenUrl = "https://claveunica.test/openid/token/",
                UserInfoUrl = "https://claveunica.test/openid/userinfo/"
            }),
            NullLogger<ClaveUnicaClient>.Instance);

        var token = await client.ExchangeCodeAsync("authorization-code", CancellationToken.None);

        Assert.AreEqual("sandbox-access-token", token);
        Assert.AreEqual(HttpMethod.Post, handler.Method);
        Assert.AreEqual("https://claveunica.test/openid/token/", handler.RequestUri);
        StringAssert.Contains(handler.FormContent, "client_id=sandbox-client-id");
        StringAssert.Contains(handler.FormContent, "client_secret=sandbox-test-secret");
        StringAssert.Contains(handler.FormContent, "redirect_uri=https%3A%2F%2Fmi.desa.anid.gob.cl%2Fauthentication%2Fcallback-clave-unica");
        StringAssert.Contains(handler.FormContent, "grant_type=authorization_code");
        StringAssert.Contains(handler.FormContent, "code=authorization-code");
        Assert.IsFalse(handler.FormContent.Contains("state=", StringComparison.Ordinal));
    }

    private sealed class StubHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
    }

    private sealed class CapturingHttpMessageHandler(string responseBody) : HttpMessageHandler
    {
        public HttpMethod? Method { get; private set; }
        public string RequestUri { get; private set; } = string.Empty;
        public string FormContent { get; private set; } = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Method = request.Method;
            RequestUri = request.RequestUri?.ToString() ?? string.Empty;
            FormContent = request.Content is null
                ? string.Empty
                : await request.Content.ReadAsStringAsync(cancellationToken);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            };
        }
    }
}
