using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Shared.Contracts;
using FluentAssertions;

namespace CompanyManager.Domain.UnitTests;

public class EmployeeTests
{
    [Theory]
    [InlineData("test@example.com", EmployeeTitle.Developer)]
    [InlineData("manager@example.com", EmployeeTitle.Manager)]
    [InlineData("tester@example.com", EmployeeTitle.Tester)]
    public void CreateNew_ValidInput_ShouldCreateEmployee(string email, EmployeeTitle title)
    {
        TimeProvider.Set(new DateTime(2023, 8, 30));
        var employee = Employee.CreateNew(email, title);

        employee.Email.Should().Be(email);
        employee.Title.Should().Be(title);
        employee.Id.Should().NotBe(Guid.Empty);
        employee.CreatedAt.Should().Be(new DateTime(2023, 8, 30));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateNew_InvalidEmail_ShouldThrowException(string invalidEmail)
    {
        Action act = () => Employee.CreateNew(invalidEmail, EmployeeTitle.Developer);

        act.Should().Throw<ArgumentException>().WithMessage("Email cannot be null or empty.");
    }
}