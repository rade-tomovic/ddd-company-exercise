using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.ValueObjects;

public record Email
{
    public Email(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(Email));
        Guard.AgainstInvalidEmail(value, nameof(Email));
        Value = value;
    }

    public string Value { get; }
}