using FluentValidation;

namespace CompanyManager.Application.Companies.AddCompany;

public class AddCompanyCommandValidator : AbstractValidator<AddCompanyCommand>
{
    public AddCompanyCommandValidator()
    {
        RuleFor(x => x.CompanyName).NotNull().NotEmpty().WithMessage("CustomerId is null or empty");
        RuleForEach(x => x.Employees).SetValidator(new EmployeeToAddValidator());
    }
}