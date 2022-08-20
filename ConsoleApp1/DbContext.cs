using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EF_DDD.Partnership;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_DDD {
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext {
        private readonly string _connectionString;
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public DbContext(string connectionString) : base()
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString is null");

            _connectionString = connectionString;
        }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return;

            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);
            if (Debugger.IsAttached) {
                optionsBuilder.UseLoggerFactory(LoggerFactory);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null)
                {
                    modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
                }
            }
            modelBuilder.ApplyConfiguration(new PartnerEmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new PartnerConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<ContractingCompany> ContractingCompanies { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Partner> Partners { get; set; }
    }
}
