using CompanyManager.Persistence.Domain.Companies;

namespace CompanyManager.Persistence.Domain.Employees;

public class EmployeeDbEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public ICollection<CompanyEmployeeDbEntity> CompanyEmployees { get; set; }
}
