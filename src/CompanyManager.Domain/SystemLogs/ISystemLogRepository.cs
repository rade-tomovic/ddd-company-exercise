using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.SystemLogs;

public interface ISystemLogRepository
{
    Task<string> AddSystemLog(SystemLog<IDomainEvent<Entity>, Entity> log);
}