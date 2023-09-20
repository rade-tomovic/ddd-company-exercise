using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyManager.Persistence.Domain.Companies;

public class CompanyDbEntityConfiguration : IEntityTypeConfiguration<CompanyDbEntity>
{
    public void Configure(EntityTypeBuilder<CompanyDbEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.Name);
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.CreatedAt).IsRequired();

        builder.HasMany(c => c.CompanyEmployees)
            .WithOne(ce => ce.Company)
            .HasForeignKey(ce => ce.CompanyId);
    }
}