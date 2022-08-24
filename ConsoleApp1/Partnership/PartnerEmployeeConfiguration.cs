using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EF_DDD.Partnership
{
    public class PartnerEmployeeConfiguration : IEntityTypeConfiguration<PartnerEmployee>
    {
        public void Configure(EntityTypeBuilder<PartnerEmployee> builder)
        {
            builder.ToTable("Person").HasKey(c => c.Id);
            builder.Property(c => c.EmployeeId).HasColumnName("EmployeeId");
            builder.HasMany(e => e.Reports).WithOne(e => e.Manager);
            builder.Property(e => e.Name).HasColumnName("Name");
            builder.Property("PartnerId").HasColumnName("CompanyId");
            builder.Property("ManagerId").HasColumnName("ManagerId");
            builder.Property<string>("Discriminator").HasColumnName("Discriminator").HasValueGenerator<DiscriminatorValueGenerator>();
        }

        public class DiscriminatorValueGenerator : ValueGenerator<string>
        {
            public override string Next(EntityEntry _) => "Contractor";

            public override bool GeneratesTemporaryValues => false;
        }
    }
}
