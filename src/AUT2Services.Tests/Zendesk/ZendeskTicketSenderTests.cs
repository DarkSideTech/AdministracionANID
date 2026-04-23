using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Tools.ZendeskManager;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace AUT2Services.Tests.Zendesk;

[TestClass]
public class ZendeskTicketSenderTests
{
    [TestMethod]
    public async Task SendTicketData_WhenZendeskReturnsSuccess_ReturnsSuccess()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("{\"ticket\":{\"id\":12345,\"status\":\"open\"}}", Encoding.UTF8, "application/json")
            });

        var sender = CreateSender(handler);

        var result = await sender.SendTicketData(new TicketDataModel
        {
            Subject = "Registro",
            Body = "Detalle",
            Priority = "normal"
        });

        Assert.IsTrue(result.Result);
        Assert.AreEqual("Ticket creado exitosamente. Codigo HTTP: 201.", result.Data);
    }

    [TestMethod]
    public async Task SendTicketData_WhenZendeskReturnsError_ReturnsFailure()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                Content = new StringContent("{\"error\":\"upstream\"}", Encoding.UTF8, "application/json")
            });

        var sender = CreateSender(handler);

        var result = await sender.SendTicketData(new TicketDataModel
        {
            Subject = "Registro",
            Body = "Detalle",
            Priority = "normal"
        });

        Assert.IsFalse(result.Result);
        Assert.AreEqual("No fue posible notificar Zendesk. Codigo HTTP: 502.", result.Data);
    }

    private static ZendeskTicketSender CreateSender(HttpMessageHandler handler)
    {
        return new ZendeskTicketSender(
            new HttpClient(handler)
            {
                BaseAddress = new Uri("https://darksidetech.zendesk.com")
            },
            Options.Create(new ZendeskSendTicketOptions
            {
                Subdomain = "https://darksidetech.zendesk.com",
                Email = "sender@example.com",
                ApiToken = "token",
                ApiUri = "/api/v2/tickets.json"
            }),
            NullLogger<ZendeskTicketSender>.Instance);
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(request));
        }
    }
}
