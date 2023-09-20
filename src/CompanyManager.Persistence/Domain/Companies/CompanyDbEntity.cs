using CompanyManager.Persistence.Domain.Employees;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyDbEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CompanyEmployeeDbEntity> CompanyEmployees { get; set; }
}
