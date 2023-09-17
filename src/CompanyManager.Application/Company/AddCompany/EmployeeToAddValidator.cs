using FluentValidation;

namespace CompanyManager.Application.Company.AddCompany;

public class EmployeeToAddValidator : AbstractValidator<EmployeeToAdd>
{
    public EmployeeToAddValidator()
    {
        RuleFor(x => x.Email).NotEmpty().When(x => x.Id == null).WithMessage("Email is null or empty, while Id is not provided");
        RuleFor(x => x.Title).NotEmpty().When(x => x.Id == null).WithMessage("Title is null or empty, while Id is not provided");
    }
}