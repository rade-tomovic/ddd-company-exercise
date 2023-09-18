namespace CompanyManager.Domain.Companies.Contracts;

public interface IEmployeeEmailUniquenessChecker
{
    Task<bool> IsUniqueAsync(string email, Guid companyId);
}