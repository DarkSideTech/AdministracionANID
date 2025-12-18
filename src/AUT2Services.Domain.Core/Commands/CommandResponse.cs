using FluentValidation.Results;

namespace AUT2Services.Domain.Core.Commands
{
    public class CommandResponse
    {
        public CommandResponse()
        {
            Timestamp = DateTimeOffset.Now;
            ValidationResult = new ValidationResult();
            Result = false;
            Data = string.Empty;
        }

        public bool Result { get; set; }
        public string Data { get; set; }
        public DateTimeOffset Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }
    }
}
