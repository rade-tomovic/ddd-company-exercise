namespace CompanyManager.Domain.Shared.Contracts;

/// <summary>
///     Provides properties to track object create/update time.
/// </summary>
public interface IAuditable
{
    /// <summary>
    ///     Gets or sets the creation date and time of the object.
    /// </summary>
    DateTime CreatedAt { get; }
}