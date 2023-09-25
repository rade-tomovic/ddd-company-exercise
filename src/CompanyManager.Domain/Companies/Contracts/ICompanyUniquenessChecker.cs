namespace CompanyManager.Domain.Companies.Contracts;

public interface ICompanyUniquenessChecker
{
    Task<bool> IsUniqueAsync(string name);
}