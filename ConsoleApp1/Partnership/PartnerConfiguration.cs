using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_DDD.Partnership
{
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("ContractingCompany").HasKey(c => c.Id);
            builder.Property(c => c.Name).HasColumnName("Name");
            builder.HasMany(c => c.Employees).WithOne(e => e.Partner);
        }
    }
}
