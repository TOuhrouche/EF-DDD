using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
            var employeeContext = new EmployeeContext(ConnectionString);
            var partnersContext = new PartnersContext(employeeContext.Database.GetDbConnection());
            var unitOfWork = new UnitOfWork();
            unitOfWork.UpdateFixed(employeeContext, partnersContext, 1);
            unitOfWork.UpdateFixed(employeeContext, partnersContext, 2);
            //await unitOfWork.UpdateUsingTransactionScope(employeeContext, partnersContext, 1);
            //await unitOfWork.UpdateUsingTransactionScope(employeeContext, partnersContext, 2);
        }

        public class UnitOfWork
        {
            public void Update(EmployeeContext employeeContext, PartnersContext partnerContext, int count)
            {
                var strategy = employeeContext.Database.CreateExecutionStrategy();
                strategy.Execute(() => { 
                    using var trans = employeeContext.Database.BeginTransaction();
                    partnerContext.Database.UseTransaction(trans.GetDbTransaction());
                    partnerContext.Partners.Add(new Partner($"John Smith {count}"));
                    employeeContext.Persons.Add(new Person() { Name = $"Richard Keno {count}" });
                    partnerContext.SaveChanges();
                    employeeContext.SaveChanges();
                    trans.Commit();
                });
            }

            public void UpdateFixed(EmployeeContext employeeContext, PartnersContext partnerContext, int count)
            {
                var strategy = employeeContext.Database.CreateExecutionStrategy();
                strategy.Execute(() => {
                    using var trans = employeeContext.Database.BeginTransaction();
                    var transactionToDispose = partnerContext.Database.UseTransaction(trans.GetDbTransaction());
                    partnerContext.Partners.Add(new Partner($"John Smith {count}"));
                    employeeContext.Persons.Add(new Person() { Name = $"Richard Keno {count}" });
                    partnerContext.SaveChanges();
                    employeeContext.SaveChanges();
                    trans.Commit();
                    transactionToDispose.Dispose();
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
