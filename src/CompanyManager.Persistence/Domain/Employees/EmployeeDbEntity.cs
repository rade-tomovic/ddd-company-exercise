using CompanyManager.Persistence.Domain.Companies;

namespace CompanyManager.Persistence.Domain.Employees;

public class EmployeeDbEntity
{
    public EmployeeDbEntity()
    {
        Companies = new HashSet<CompanyDbEntity>();
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<CompanyDbEntity> Companies { get; set; }
}
