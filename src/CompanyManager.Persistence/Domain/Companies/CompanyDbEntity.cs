using CompanyManager.Persistence.Domain.Employees;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyDbEntity
{
    public CompanyDbEntity()
    {
        Employees = new HashSet<EmployeeDbEntity>();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<EmployeeDbEntity> Employees { get; set; }
}
