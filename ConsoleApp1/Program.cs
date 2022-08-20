using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EF_DDD
{
    internal class Program
    {
        public static string ConnectionString =
            "Data Source=localhost;Initial Catalog=EF_DDD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";
        static void Main(string[] args)
        {
            var context = new DbContext(ConnectionString);
            //Seed(context);
            var contractingCompany = context.ContractingCompanies.First();
            var partner = context.Partners.First();
        }

        private static void Seed(DbContext context)
        {
            var contractingCompany = new ContractingCompany("Contracting AS.");
            var sarah = new Contractor() {Name = "Sarah Johns", ContractorId = 2233};
            var julie = new Contractor() {Name = "Julie Smith", ContractorId = 1334, Manager = sarah};
            contractingCompany.Add(sarah);
            contractingCompany.Add(julie);
            context.ContractingCompanies.Add(contractingCompany);
            context.Employees.Add(new Employee() {Name = "John Bek", Grade = 2});
            context.SaveChanges();
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
