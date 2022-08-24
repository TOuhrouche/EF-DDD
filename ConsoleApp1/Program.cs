using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EF_DDD.Partnership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;

namespace EF_DDD
{
    internal class Program
    {
        public static string ConnectionString =
            "Data Source=localhost;Initial Catalog=EF_DDD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";
        static async Task Main(string[] args)
        {
            var transactionContext = new EmployeeContext(ConnectionString);
            var partnersContext = new PartnersContext(transactionContext.Database.GetDbConnection());
            await TestResilientTransaction(transactionContext, partnersContext, 1);
            await TestResilientTransaction(transactionContext, partnersContext, 2);
        }

        private static async Task TestResilientTransaction(EmployeeContext employeeContext, PartnersContext partnerContext, int count)
        {
            partnerContext.Partners.Add(new Partner($"John Smith {count}"));
            employeeContext.Persons.Add(new Person() {Name = $"Richard Keno {count}"});

            // Use of an EF Core resiliency strategy when using multiple DbContexts
            // within an explicit BeginTransaction():
            // https://docs.microsoft.com/ef/core/miscellaneous/connection-resiliency
            await ResilientTransaction.New(employeeContext).ExecuteAsync(async (transaction) => {
                await partnerContext.Database.UseTransactionAsync(transaction.GetDbTransaction());
                await partnerContext.SaveChangesAsync();
                await employeeContext.SaveChangesAsync();
            });
        }

        public class ResilientTransaction
        {
            private readonly Microsoft.EntityFrameworkCore.DbContext _context;
            private ResilientTransaction(Microsoft.EntityFrameworkCore.DbContext context) =>
                _context = context ?? throw new ArgumentNullException(nameof(context));

            public static ResilientTransaction New(Microsoft.EntityFrameworkCore.DbContext context) => new ResilientTransaction(context);

            public async Task ExecuteAsync(Func<IDbContextTransaction, Task> action)
            {
                // Use of an EF Core resiliency strategy when using multiple DbContexts
                // within an explicit BeginTransaction():
                // https://docs.microsoft.com/ef/core/miscellaneous/connection-resiliency
                var strategy = _context.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    await action(transaction);
                    await transaction.CommitAsync();
                });
            }
        }

        public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<DbContext>
        {
            public DbContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<DbContext>();
                builder.UseSqlServer(ConnectionString);
                return new DbContext(builder.Options);
            }
        }
    }
}
