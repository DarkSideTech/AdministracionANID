using AUT2Services.Domain.Core.Models;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Services.API.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AUT2Services.Tests.Configurations;

[TestClass]
public class ExternalIntegrationOptionsConfigTests
{
    [TestMethod]
    public void AddExternalIntegrationOptions_WithValidConfiguration_BindsOptions()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            [$"{SendEmailOptions.EmailOptionsKey}:SmtpClient"] = "smtp.gmail.com",
            [$"{SendEmailOptions.EmailOptionsKey}:Port"] = "587",
            [$"{SendEmailOptions.EmailOptionsKey}:Remitente"] = "sender@example.com",
            [$"{SendEmailOptions.EmailOptionsKey}:Password"] = "secret",
            ["EmailValidation:ConfirmationUrlBase"] = "http://localhost:4210/authentication/confirm-email",
            ["EmailValidation:ResendCooldownMinutes"] = "2",
            ["EmailValidation:ResendRequestsPerWindow"] = "5",
            ["EmailValidation:ResendWindowMinutes"] = "10",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Subdomain"] = "https://darksidetech.zendesk.com",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Email"] = "sender@example.com",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiToken"] = "token",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiUri"] = "/api/v2/tickets.json"
        });

        services.AddExternalIntegrationOptions(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        var emailOptions = serviceProvider.GetRequiredService<IOptions<SendEmailOptions>>().Value;
        var emailValidationOptions = serviceProvider.GetRequiredService<IOptions<EmailValidationOptions>>().Value;
        var zendeskOptions = serviceProvider.GetRequiredService<IOptions<ZendeskSendTicketOptions>>().Value;

        Assert.AreEqual("smtp.gmail.com", emailOptions.SmtpClient);
        Assert.AreEqual("http://localhost:4210/authentication/confirm-email", emailValidationOptions.ConfirmationUrlBase);
        Assert.AreEqual("https://darksidetech.zendesk.com", zendeskOptions.Subdomain);
    }

    [TestMethod]
    public void AddExternalIntegrationOptions_WithMissingEmailPassword_ThrowsValidationException()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            [$"{SendEmailOptions.EmailOptionsKey}:SmtpClient"] = "smtp.gmail.com",
            [$"{SendEmailOptions.EmailOptionsKey}:Port"] = "587",
            [$"{SendEmailOptions.EmailOptionsKey}:Remitente"] = "sender@example.com",
            ["EmailValidation:ConfirmationUrlBase"] = "http://localhost:4210/authentication/confirm-email",
            ["EmailValidation:ResendCooldownMinutes"] = "2",
            ["EmailValidation:ResendRequestsPerWindow"] = "5",
            ["EmailValidation:ResendWindowMinutes"] = "10",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Subdomain"] = "https://darksidetech.zendesk.com",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Email"] = "sender@example.com",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiToken"] = "token",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiUri"] = "/api/v2/tickets.json"
        });

        services.AddExternalIntegrationOptions(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        try
        {
            _ = serviceProvider.GetRequiredService<IOptions<SendEmailOptions>>().Value;
            Assert.Fail("Se esperaba OptionsValidationException para SendEmailOptions.");
        }
        catch (OptionsValidationException)
        {
        }
    }

    [TestMethod]
    public void AddExternalIntegrationOptions_WithInvalidZendeskSubdomain_ThrowsValidationException()
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            [$"{SendEmailOptions.EmailOptionsKey}:SmtpClient"] = "smtp.gmail.com",
            [$"{SendEmailOptions.EmailOptionsKey}:Port"] = "587",
            [$"{SendEmailOptions.EmailOptionsKey}:Remitente"] = "sender@example.com",
            [$"{SendEmailOptions.EmailOptionsKey}:Password"] = "secret",
            ["EmailValidation:ConfirmationUrlBase"] = "http://localhost:4210/authentication/confirm-email",
            ["EmailValidation:ResendCooldownMinutes"] = "2",
            ["EmailValidation:ResendRequestsPerWindow"] = "5",
            ["EmailValidation:ResendWindowMinutes"] = "10",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Subdomain"] = "notaurl",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:Email"] = "sender@example.com",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiToken"] = "token",
            [$"{ZendeskSendTicketOptions.ZendeskTicketOptionsKey}:ApiUri"] = "/api/v2/tickets.json"
        });

        services.AddExternalIntegrationOptions(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        try
        {
            _ = serviceProvider.GetRequiredService<IOptions<ZendeskSendTicketOptions>>().Value;
            Assert.Fail("Se esperaba OptionsValidationException para ZendeskSendTicketOptions.");
        }
        catch (OptionsValidationException)
        {
        }
    }

    private static IConfiguration BuildConfiguration(IDictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
