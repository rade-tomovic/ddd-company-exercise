using CompanyManager.Domain.Companies;
using CompanyManager.Domain.Companies.Contracts;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Domain.Shared.Exceptions;
using FluentAssertions;
using Moq;

namespace CompanyManager.Domain.UnitTests;

public class CompanyTests
{
    [Fact]
    public async Task CreateNew_CompanyWithUniqueName_ShouldReturnNewCompany()
    {
        Mock<ICompanyUniquenessChecker> mockUniquenessChecker = new();
        mockUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("UniqueName", mockUniquenessChecker.Object);

        company.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateNew_CompanyWithDuplicateName_ShouldThrowException()
    {
        Mock<ICompanyUniquenessChecker> mockUniquenessChecker = new();
        mockUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

        Func<Task> act = async () => await Company.CreateNew("DuplicateName", mockUniquenessChecker.Object);

        await act.Should().ThrowAsync<BusinessRuleAsyncViolationException>()
            .WithMessage("Company name must be unique. Company name: DuplicateName");
    }

    [Theory]
    [InlineData("dev@example.com", EmployeeTitle.Developer)]
    [InlineData("mgr@example.com", EmployeeTitle.Manager)]
    public async Task AddEmployee_WithUniqueEmailAndTitle_ShouldReturnEmployeeId(string email, EmployeeTitle title)
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeEmailUniquenessChecker> mockEmailChecker = new();
        mockEmailChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeTitleWithinCompanyUniquenessChecker> mockTitleChecker = new();
        mockTitleChecker.Setup(x => x.IsUniqueAsync(It.IsAny<EmployeeTitle>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        EmployeeId employeeId = await company.AddEmployee(email, title, mockEmailChecker.Object, mockTitleChecker.Object);

        employeeId.Should().NotBeNull();
    }

    [Fact]
    public async Task AddEmployee_WithDuplicateEmail_ShouldThrowException()
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeEmailUniquenessChecker> mockEmailChecker = new();
        mockEmailChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(false));

        Mock<IEmployeeTitleWithinCompanyUniquenessChecker> mockTitleChecker = new();
        mockTitleChecker.Setup(x => x.IsUniqueAsync(It.IsAny<EmployeeTitle>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        Func<Task> act = async () =>
            await company.AddEmployee("duplicate@example.com", EmployeeTitle.Developer, mockEmailChecker.Object, mockTitleChecker.Object);

        await act.Should().ThrowAsync<BusinessRuleAsyncViolationException>()
            .WithMessage("Employee email must be unique. Email: duplicate@example.com");
    }

    [Fact]
    public async Task AddEmployee_WithDuplicateTitle_ShouldThrowException()
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeEmailUniquenessChecker> mockEmailChecker = new();
        mockEmailChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeTitleWithinCompanyUniquenessChecker> mockTitleChecker = new();
        mockTitleChecker.Setup(x => x.IsUniqueAsync(It.IsAny<EmployeeTitle>())).Returns(Task.FromResult(false));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        Func<Task> act = async () =>
            await company.AddEmployee("dev@example.com", EmployeeTitle.Developer, mockEmailChecker.Object, mockTitleChecker.Object);

        await act.Should().ThrowAsync<BusinessRuleAsyncViolationException>().WithMessage("Employee title must be unique within company.");
    }

    [Fact]
    public async Task AddEmployee_WithValidDetails_ShouldAddEmployeeToCompany()
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeEmailUniquenessChecker> mockEmailChecker = new();
        mockEmailChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeTitleWithinCompanyUniquenessChecker> mockTitleChecker = new();
        mockTitleChecker.Setup(x => x.IsUniqueAsync(It.IsAny<EmployeeTitle>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        EmployeeId employeeId = await company.AddEmployee("dev@example.com", EmployeeTitle.Developer, mockEmailChecker.Object,
            mockTitleChecker.Object);

        employeeId.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateNewCompany_WithValidDetails_ShouldRaiseCompanyAddedEvent()
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        company.DomainEvents.Should().ContainSingle(e => e is CompanyAddedEvent);
    }

    [Fact]
    public async Task AddEmployee_WithValidDetails_ShouldRaiseEmployeeAddedEvent()
    {
        Mock<ICompanyUniquenessChecker> mockCompanyUniquenessChecker = new();
        mockCompanyUniquenessChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeEmailUniquenessChecker> mockEmailChecker = new();
        mockEmailChecker.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).Returns(Task.FromResult(true));

        Mock<IEmployeeTitleWithinCompanyUniquenessChecker> mockTitleChecker = new();
        mockTitleChecker.Setup(x => x.IsUniqueAsync(It.IsAny<EmployeeTitle>())).Returns(Task.FromResult(true));

        var company = await Company.CreateNew("CompanyName", mockCompanyUniquenessChecker.Object);

        await company.AddEmployee("dev@example.com", EmployeeTitle.Developer, mockEmailChecker.Object, mockTitleChecker.Object);

        company.DomainEvents.Should().Contain(e => e is EmployeeAddedEvent);
    }
}