namespace AUT2Services.Domain.Core.Models;

public class ZendeskSendTicketOptions
{
    public const string ZendeskTicketOptionsKey = "ZendeskSendTicketOptions";

    public string Subdomain { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ApiToken { get; set; } = string.Empty;
    public string ApiUri { get; set; } = "/api/v2/tickets.json";
}
