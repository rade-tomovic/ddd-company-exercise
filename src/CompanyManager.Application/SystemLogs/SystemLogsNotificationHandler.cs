using CompanyManager.Domain.SystemLogs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CompanyManager.Application.SystemLogs;

public class SystemLogsNotificationHandler : INotificationHandler<SystemLogNotification>
{
    private readonly ILogger<SystemLogsNotificationHandler> _logger;
    private readonly ISystemLogRepository _repository;

    public SystemLogsNotificationHandler(ISystemLogRepository repository, ILogger<SystemLogsNotificationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(SystemLogNotification notification, CancellationToken cancellationToken)
    {
        string result = await _repository.AddSystemLog(notification.SystemLog);

        if (!string.IsNullOrWhiteSpace(result))
            _logger.LogInformation($"System log for event {notification.SystemLog.ResourceType} successfully saved");
        else
            _logger.LogError($"System log saving failed for event {notification.SystemLog.ResourceType}");
    }
}