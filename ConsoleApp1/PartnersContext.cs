using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EF_DDD {
    public class PartnersContext : BaseContext {
        public PartnersContext(string connectionString) : base(connectionString) { }

        public PartnersContext(DbContextOptions<PartnersContext> options) : base(options) { }

        public PartnersContext(DbConnection dbConnection) : base(dbConnection) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null)
                {
                    modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
                }
            }
            modelBuilder.Entity<Partner>().ToTable("ContractingCompany").HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Partner> Partners { get; set; }
    }
}
