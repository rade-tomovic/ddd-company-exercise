using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;

namespace CompanyManager.Domain.SystemLogs;

public interface ISystemLogRepository
{
    Task<Guid> AddSystemLog(SystemLog<IDomainEvent<Entity>, Entity> log);
}