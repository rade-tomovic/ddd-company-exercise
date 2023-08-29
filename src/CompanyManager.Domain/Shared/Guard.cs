namespace CompanyManager.Domain.Shared;

public static class Guard
{
    public static void AgainstEmptyGuid(Guid value, string name)
    {
        if (value == Guid.Empty) throw new ArgumentException($"{name} cannot be an empty GUID.");
    }
}