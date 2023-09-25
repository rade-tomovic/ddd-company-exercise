using AutoMapper;
using CompanyManager.Domain.Companies;
using CompanyManager.Persistence.Domain.Companies;

namespace CompanyManager.Persistence.EntityMapping;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CompanyDbEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Employees, opt => opt.Ignore());

        CreateMap<CompanyDbEntity, Company>()
            .ConstructUsing(src => Company.CreateNewWithoutChecking(src.Name, src.CreatedAt, src.Id));
    }
}