using AUT2Services.Domain.Core.Messaging;
using AUT2Services.Domain.Core.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AUT2Services.Infra.Tools.ZendeskManager;
public class ZendeskTicketSender(
       IOptions<ZendeskSendTicketOptions> zendeskSendTicketOptions
       ) : ITicketDataSender
{
    private static readonly HttpClient client = new();
    private readonly ZendeskSendTicketOptions zendeskSendTicketOptions = zendeskSendTicketOptions.Value;

    public async Task<ResultModel> SendTicketData(TicketDataModel ticketDataModel)
    {
        var result = new ResultModel()
        {
            Result = false,
            Data = string.Empty
        };

        string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{this.zendeskSendTicketOptions.Email}/token:{this.zendeskSendTicketOptions.ApiToken}"));

        // 2. Estructura del Ticket (JSON)
        var ticketData = new
        {
            ticket = new
            {
                subject = ticketDataModel.Subject,
                comment = new { body = ticketDataModel.Body },
                priority = ticketDataModel.Priority
            }
        };

        string jsonPayload = JsonSerializer.Serialize(ticketData);

        // 3. Preparar y enviar la solicitud
        var request = new HttpRequestMessage(HttpMethod.Post, $"{this.zendeskSendTicketOptions.Subdomain}{this.zendeskSendTicketOptions.ApiUri}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.SendAsync(request);
            string resultResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Ticket creado exitosamente.");
                result.Result = true;
                result.Data = JsonSerializer.Serialize(resultResponse);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {result}");
                result.Data = $"Error: {response.StatusCode} - {result}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error de conexión: {ex.Message}");
        }

        return result;
    }
}
