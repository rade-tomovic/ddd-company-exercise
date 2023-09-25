namespace CompanyManager.Application.Core;

public interface IExecutionContextAccessor
{
    Guid CorrelationId { get; }

    bool IsAvailable { get; }
}