using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.SystemLogs;
using MediatR;

namespace CompanyManager.Application.SystemLogs;

public class SystemLogNotification : INotification
{
    public SystemLogNotification(SystemLog<IDomainEvent<Entity>, Entity> systemLog)
    {
        SystemLog = systemLog;
    }

    public SystemLog<IDomainEvent<Entity>, Entity> SystemLog { get; }
}