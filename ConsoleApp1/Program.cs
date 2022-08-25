using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
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
        static void Main(string[] args)
        {
            var employeeContext = new EmployeeContext(ConnectionString);
            var partnersContext = new PartnersContext(employeeContext.Database.GetDbConnection());
            var unitOfWork = new UnitOfWork();
            unitOfWork.Update(employeeContext, partnersContext, 1);
            unitOfWork.Update(employeeContext, partnersContext, 2);

        }

        public class UnitOfWork
        {
            public void Update(EmployeeContext employeeContext, PartnersContext partnerContext, int count)
            {
                partnerContext.Partners.Add(new Partner($"John Smith {count}"));
                employeeContext.Persons.Add(new Person() { Name = $"Richard Keno {count}" });

                using var trans = employeeContext.Database.BeginTransaction();
                partnerContext.Database.UseTransaction(trans.GetDbTransaction());
                partnerContext.SaveChanges();
                employeeContext.SaveChanges();
                trans.Commit();
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
