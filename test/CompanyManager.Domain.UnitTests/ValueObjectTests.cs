using System.Diagnostics.CodeAnalysis;
using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace CompanyManager.Tests;

[SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
public class ValueObjectTests
{
    [Theory]
    [InlineData("email@example.com")]
    [InlineData("email.test@example.com")]
    [InlineData("email+test@example.com")]
    [InlineData("email.test+test@example.com")]
    [InlineData("email.test+test@example.international")]
    public void Email_InitWithValidEmail_ShouldSetEmailValue(string validEmail)
    {
        var email = new Email(validEmail);

        email.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("", "Email cannot be null or empty.")]
    [InlineData("    ", "Email cannot be null or empty.")]
    [InlineData(null, "Email cannot be null or empty.")]
    public void Email_InitWithEmptyOrNull_ShouldThrowException(string invalidEmail, string expectedMessage)
    {
        Action action = () => new Email(invalidEmail);

        action.Should().Throw<ArgumentException>().WithMessage(expectedMessage);
    }

    [Theory]
    [InlineData("invalidEmail", "Email is not a valid email address.")]
    [InlineData("invalid@Email", "Email is not a valid email address.")]
    public void Email_InitWithInvalidFormat_ShouldThrowException(string invalidEmail, string expectedMessage)
    {
        Action action = () => new Email(invalidEmail);

        action.Should().Throw<ArgumentException>().WithMessage(expectedMessage);
    }

    [Fact]
    public void EmployeeId_InitWithEmptyGuid_ShouldThrowException()
    {
        Action action = () => new EmployeeId(Guid.Empty);

        action.Should().Throw<ArgumentException>().WithMessage("EmployeeId cannot be an empty GUID.");
    }

    [Fact]
    public void EmployeeId_InitWithValidGuid_ShouldSetIdValue()
    {
        var validGuid = Guid.NewGuid();
        var employeeId = new EmployeeId(validGuid);

        employeeId.Value.Should().Be(validGuid);
    }

    [Fact]
    public void CompanyId_InitWithEmptyGuid_ShouldThrowException()
    {
        Action action = () => { new CompanyId(Guid.Empty); };

        action.Should().Throw<ArgumentException>().WithMessage("CompanyId cannot be an empty GUID.");
    }

    [Fact]
    public void CompanyId_InitWithValidGuid_ShouldSetIdValue()
    {
        var validGuid = Guid.NewGuid();
        var companyId = new CompanyId(validGuid);

        companyId.Value.Should().Be(validGuid);
    }
}