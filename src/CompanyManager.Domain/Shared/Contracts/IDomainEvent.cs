using CompanyManager.Domain.Shared.Core;
using MediatR;

namespace CompanyManager.Domain.Shared.Contracts;

/// <summary>
/// Defines the structure for domain events related to an entity.
/// </summary>
/// <typeparam name="TEntity">The type of the related entity, must be a subtype of <see cref="Entity"/>.</typeparam>
public interface IDomainEvent<out TEntity> : INotification where TEntity : Entity
{
    /// <summary>
    /// Gets the date and time when the event occurred.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Gets a comment related to the event. Often used for human-readable messages.
    /// For example, "new employee %employee.email% was created."
    /// </summary>
    string Comment { get; }

    /// <summary>
    /// Gets the resource type involved in the event. This is generally the name of the entity.
    /// For example, "Employee" or "Company."
    /// </summary>
    string ResourceType { get; }

    /// <summary>
    /// Gets the action that triggered the event. Commonly used values include "create" and "update."
    /// </summary>
    string EventAction { get; }

    /// <summary>
    /// Gets the entity associated with the domain event. This object often includes attributes 
    /// or a changeset that provides more details about what was affected by the event.
    /// </summary>
    TEntity Entity { get; }
}