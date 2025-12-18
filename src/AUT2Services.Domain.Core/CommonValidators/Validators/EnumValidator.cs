using FluentValidation;
using System.Reflection;

namespace AUT2Services.Domain.Core.CommonValidators.Validators;

public static partial class CommonValidator
{
    public static bool EnumerationValidator(Type type, string enumerationValue)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
            .Select(static x => (string)x.GetRawConstantValue()!)
            .ToList()
            .Exists(item => item == enumerationValue);
    }
}
