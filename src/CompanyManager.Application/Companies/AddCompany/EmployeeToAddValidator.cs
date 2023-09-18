using FluentValidation;

namespace CompanyManager.Application.Company.AddCompany;

public class EmployeeToAddValidator : AbstractValidator<EmployeeToAdd>
{
    public EmployeeToAddValidator()
    {
        RuleFor(x => x.Id).NotNull().When(x => x.Email == null && x.Title == null).WithMessage("Id is null, while Email and Title are not provided");
        RuleFor(x => x.Email).NotEmpty().When(x => x.Id == null).WithMessage("Email is null or empty, while Id is not provided");
        RuleFor(x => x.Title).NotEmpty().When(x => x.Id == null).WithMessage("Title is null or empty, while Id is not provided");
    }
}