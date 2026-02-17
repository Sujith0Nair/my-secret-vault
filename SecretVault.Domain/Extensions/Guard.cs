namespace SecretVault.Domain.Extensions;

public static class Guard
{
    public static void AgainstNullOrWhiteSpace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{parameterName} cannot be null or whitespace", parameterName);
        }
    }
    
    public static void AgainstEmptyGuid(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException($"{parameterName} cannot be empty", parameterName);
        }
    }

    public static void AgainstEnumValue<T>(T input, T notDesired, string parameterName) where T : Enum
    {
        if (Equals(input, notDesired))
        {
            throw new ArgumentException($"{parameterName} cannot be '{notDesired}'", parameterName);
        }
    }
    
    public static void AgainstNullObject(object input, string parameterName)
    {
        if (input == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }
}