using CompanyManager.Domain.Shared.Contracts;
using CompanyManager.Domain.Shared.Core;
using CompanyManager.Domain.SystemLogs;

namespace CompanyManager.Persistence.Domain.SystemLogs;

public class SystemLogRepository : ISystemLogRepository
{
    public async Task<Guid> AddSystemLog(SystemLog<IDomainEvent<Entity>, Entity> log)
    {
        return default;
    }
}