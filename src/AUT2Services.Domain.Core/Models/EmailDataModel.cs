using AUT2Services.Domain.Core.Enumerations;

namespace AUT2Services.Domain.Core.Models;

public class EmailDataModel
{
    public IList<AddressMailbox> FromMailboxAddresses { get; set; } = new List<AddressMailbox>();
    public IList<AddressMailbox> ToMailboxAddresses { get; set; } = new List<AddressMailbox>();
    public string Subject { get; set; } = string.Empty;
    public string BodyType { get; set; } = EnumEmailBodyType.TEXT_BODY;
    public string Body { get; set; } = string.Empty;
}

public class AddressMailbox
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}
