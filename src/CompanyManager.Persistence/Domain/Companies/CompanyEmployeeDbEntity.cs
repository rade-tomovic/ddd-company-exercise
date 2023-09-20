using CompanyManager.Persistence.Domain.Employees;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyEmployeeDbEntity
{
    public Guid CompanyId { get; set; }
    public CompanyDbEntity Company { get; set; }

    public Guid EmployeeId { get; set; }
    public EmployeeDbEntity Employee { get; set; }
}