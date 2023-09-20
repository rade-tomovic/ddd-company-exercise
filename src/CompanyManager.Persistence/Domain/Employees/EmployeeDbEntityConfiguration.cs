using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManager.Persistence.Domain.Employees;

public class EmployeeDbEntityConfiguration : IEntityTypeConfiguration<EmployeeDbEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeDbEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.Title);

        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.Title).IsRequired();

        builder.HasMany(e => e.CompanyEmployees)
            .WithOne(ce => ce.Employee)
            .HasForeignKey(ce => ce.EmployeeId);
    }
}