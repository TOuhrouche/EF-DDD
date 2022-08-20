using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF_DDD.Partnership
{
    public class PartnerEmployeeConfiguration : IEntityTypeConfiguration<PartnerEmployee>
    {
        public void Configure(EntityTypeBuilder<PartnerEmployee> builder)
        {
            builder.ToTable("Person");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("Id");
            builder.Property(c => c.ContractorId).HasColumnName("ContractorId");
            builder.Property<int?>("PartnerId").HasColumnName("CompanyId");
            builder.HasOne<Contractor>().WithOne().HasForeignKey<PartnerEmployee>(c => c.Id);
        }
    }
}
