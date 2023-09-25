using AutoMapper;
using CompanyManager.Domain.Companies;
using CompanyManager.Persistence.Domain.Companies;
using CompanyManager.Persistence.Domain.Employees;

namespace CompanyManager.Persistence.EntityMapping;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyDbEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.CompanyEmployees, opt => opt.MapFrom(src => src.Employees.Select(e => new CompanyEmployeeDbEntity
            {
                EmployeeId = e.Id,
                CompanyId = src.Id,
                Employee = new EmployeeDbEntity
                {
                    Id = e.Id,
                    Email = e.Email,
                    Title = e.Title.ToString()
                }
            })));

        CreateMap<CompanyDbEntity, Company>()
            .ConstructUsing(src => Company.CreateNewWithoutChecking(src.Name, src.CreatedAt, src.Id));
    }
}