using CompanyManager.Application.Core.Commands;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommandHandler : ICommandHandler<AddEmployeeCommand, Guid>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeEmailUniquenessChecker _employeeEmailUniquenessChecker;
    private readonly IEmployeeTitleWithinCompanyUniquenessChecker _employeeTitleWithinCompanyUniquenessChecker;

    public AddEmployeeCommandHandler(IEmployeeEmailUniquenessChecker employeeEmailUniquenessChecker,
        IEmployeeTitleWithinCompanyUniquenessChecker employeeTitleWithinCompanyUniquenessChecker,
        ICompanyRepository companyRepository)
    {
        _employeeEmailUniquenessChecker = employeeEmailUniquenessChecker;
        _employeeTitleWithinCompanyUniquenessChecker = employeeTitleWithinCompanyUniquenessChecker;
        _companyRepository = companyRepository;
    }

    public async Task<Guid> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = Employee.CreateNew(request.Email, request.Title);
        IAsyncEnumerable<Company> companies = await _companyRepository.GetByIdsAsync(request.CompanyIds);

        await foreach (Company company in companies.WithCancellation(cancellationToken))
        {
            await company.AddEmployee(employee.Email, employee.Title, _employeeEmailUniquenessChecker,
                _employeeTitleWithinCompanyUniquenessChecker);

            await _companyRepository.UpdateAsync(company);
        }

        return employee.Id;
    }
}