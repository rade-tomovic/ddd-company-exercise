using FluentValidation;

namespace CompanyManager.Application.Employees.AddEmployee;

public class AddEmployeeCommandValidator : AbstractValidator<AddEmployeeCommand>
{
    public AddEmployeeCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Employee email is null or empty");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Employee title is null or empty");
        RuleFor(x => x.CompanyIds).NotEmpty().WithMessage("CompanyIds are null or it is empty collection");
        RuleForEach(x => x.CompanyIds).NotEqual(Guid.Empty).WithMessage("Company Ids contain empty Guid");
    }
}