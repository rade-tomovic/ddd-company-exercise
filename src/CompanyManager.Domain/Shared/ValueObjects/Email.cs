using CompanyManager.Domain.Shared.Contracts;

namespace CompanyManager.Domain.Shared.ValueObjects;

public record Email(string Value)
{
    private readonly string _value = string.Empty;

    public string Value
    {
        get => _value;
        init
        {
            Guard.AgainstNullOrEmpty(value, nameof(Email));
            Guard.AgainstInvalidEmail(value, nameof(Email));
            _value = value;
        }
    }
}