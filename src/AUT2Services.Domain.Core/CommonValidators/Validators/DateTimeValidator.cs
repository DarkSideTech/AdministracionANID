namespace AUT2Services.Domain.Core.CommonValidators.Validators;

public static partial class CommonValidator
{
    public static bool DateTimeValidator(object fechaParaValidar)
    {
        if (!DateTime.TryParse(fechaParaValidar.ToString(), out _))
        {
            return false;
        }
        return true;
    }
}
