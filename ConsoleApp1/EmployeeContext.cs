using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace EF_DDD {
    public class EmployeeContext : BaseContext {
        public EmployeeContext(string connectionString) : base(connectionString) { }

        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

        public EmployeeContext(DbConnection dbConnection): base(dbConnection) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null)
                {
                    modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
    }
}
