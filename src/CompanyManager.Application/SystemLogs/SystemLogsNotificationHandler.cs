using CompanyManager.Application.Core;
using CompanyManager.Domain.SystemLogs;
using MediatR;
using Serilog;

namespace CompanyManager.Application.SystemLogs;

public class SystemLogsNotificationHandler : INotificationHandler<SystemLogNotification>
{
    private readonly ILogger _logger;
    private readonly ISystemLogRepository _repository;
    private readonly IExecutionContextAccessor _contextAccessor;

    public SystemLogsNotificationHandler(ISystemLogRepository repository, ILogger logger, IExecutionContextAccessor contextAccessor)
    {
        _repository = repository;
        _logger = logger;
        _contextAccessor = contextAccessor;
    }

    public async Task Handle(SystemLogNotification notification, CancellationToken cancellationToken)
    {
        string result = await _repository.AddSystemLog(notification.SystemLog);

        if (!string.IsNullOrWhiteSpace(result))
            _logger.Information($"System log for event {notification.SystemLog.ResourceType} successfully saved. Correlation ID: {_contextAccessor.CorrelationId}. System Log ID: {result}");
        else
            _logger.Error($"System log saving failed for event {notification.SystemLog.ResourceType}. Correlation ID: {_contextAccessor.CorrelationId}");
    }
}