using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AUT2Services.Infra.Tools.ZendeskManager;

public class ZendeskTicketSender : ITicketDataSender
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly ZendeskSendTicketOptions zendeskSendTicketOptions;
    private readonly ILogger<ZendeskTicketSender> logger;

    public ZendeskTicketSender(
        HttpClient httpClient,
        IOptions<ZendeskSendTicketOptions> zendeskSendTicketOptions,
        ILogger<ZendeskTicketSender> logger)
    {
        this.httpClient = httpClient;
        this.zendeskSendTicketOptions = zendeskSendTicketOptions.Value;
        this.logger = logger;
    }

    public async Task<ResultModel> SendTicketData(TicketDataModel ticketDataModel)
    {
        var result = new ResultModel()
        {
            Result = false,
            Data = string.Empty
        };

        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{zendeskSendTicketOptions.Email}/token:{zendeskSendTicketOptions.ApiToken}"));
        var payload = new ZendeskTicketCreateRequest(
            new ZendeskTicketPayload(
                ticketDataModel.Subject,
                new ZendeskTicketComment(ticketDataModel.Body),
                ticketDataModel.Priority));

        using var request = new HttpRequestMessage(HttpMethod.Post, NormalizeApiUri(zendeskSendTicketOptions.ApiUri));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        request.Content = new StringContent(JsonSerializer.Serialize(payload, JsonSerializerOptions), Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var zendeskResponse = TryDeserializeResponse(responseBody);
                logger.LogInformation(
                    "Ticket Zendesk creado exitosamente. TicketId: {TicketId}, StatusCode: {StatusCode}.",
                    zendeskResponse?.Ticket?.Id,
                    (int)response.StatusCode);

                result.Result = true;
                result.Data = $"Ticket creado exitosamente. Codigo HTTP: {(int)response.StatusCode}.";
                return result;
            }

            logger.LogError(
                "Zendesk respondio error. StatusCode: {StatusCode}. ResponseBody: {ResponseBody}",
                (int)response.StatusCode,
                responseBody);

            result.Data = $"No fue posible notificar Zendesk. Codigo HTTP: {(int)response.StatusCode}.";
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error de conexion al intentar crear ticket en Zendesk.");
            result.Data = $"No fue posible conectar con Zendesk. Detalle: {ex.Message}";
            return result;
        }
    }

    private static ZendeskTicketCreateResponse? TryDeserializeResponse(string responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ZendeskTicketCreateResponse>(responseBody, JsonSerializerOptions);
        }
        catch
        {
            return null;
        }
    }

    private static string NormalizeApiUri(string apiUri)
    {
        if (string.IsNullOrWhiteSpace(apiUri))
        {
            return "/api/v2/tickets.json";
        }

        return apiUri.StartsWith('/') ? apiUri : $"/{apiUri}";
    }

    private sealed record ZendeskTicketCreateRequest(
        [property: JsonPropertyName("ticket")] ZendeskTicketPayload Ticket);

    private sealed record ZendeskTicketPayload(
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("comment")] ZendeskTicketComment Comment,
        [property: JsonPropertyName("priority")] string Priority);

    private sealed record ZendeskTicketComment(
        [property: JsonPropertyName("body")] string Body);

    private sealed record ZendeskTicketCreateResponse(
        [property: JsonPropertyName("ticket")] ZendeskTicketResponsePayload? Ticket);

    private sealed record ZendeskTicketResponsePayload(
        [property: JsonPropertyName("id")] long? Id,
        [property: JsonPropertyName("status")] string? Status);
}
