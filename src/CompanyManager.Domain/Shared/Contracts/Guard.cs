using System.Text.RegularExpressions;

namespace CompanyManager.Domain.Shared.Contracts;

public static class Guard
{
    public static void AgainstEmptyGuid(Guid value, string name)
    {
        if (value == Guid.Empty) throw new ArgumentException($"{name} cannot be an empty GUID.");
    }

    public static void AgainstNullOrWhiteSpace(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{name} cannot be null or empty.");
    }

    public static void AgainstInvalidEmail(string value, string name)
    {
        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            throw new ArgumentException($"{name} is not a valid email address.");
    }
}