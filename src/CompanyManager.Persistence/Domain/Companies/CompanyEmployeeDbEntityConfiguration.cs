using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyEmployeeDbEntityConfiguration : IEntityTypeConfiguration<CompanyEmployeeDbEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEmployeeDbEntity> builder)
    {
        builder.ToTable("CompanyEmployee");
        builder.HasKey(ce => new { ce.CompanyId, ce.EmployeeId});
    }
}