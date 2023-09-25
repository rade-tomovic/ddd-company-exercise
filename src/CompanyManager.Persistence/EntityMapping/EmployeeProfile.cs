using AutoMapper;
using CompanyManager.Domain.Companies.Employees;
using CompanyManager.Persistence.Domain.Employees;

namespace CompanyManager.Persistence.EntityMapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDbEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Companies, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.ToString()));

        CreateMap<EmployeeDbEntity, Employee>().ConstructUsing(src =>
            Employee.CreateNewWithoutChecking(src.Email, Enum.Parse<EmployeeTitle>(src.Title), src.CreatedAt, src.Id));
    }
}