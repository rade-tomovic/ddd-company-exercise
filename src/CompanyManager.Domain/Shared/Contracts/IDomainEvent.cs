using MediatR;

namespace CompanyManager.Domain.Shared.Contracts;

public interface IDomainEvent : INotification
{
    /// <summary>
    ///     Gets the date and time when the event occurred.
    /// </summary>
    DateTime OccurredOn { get; }
}