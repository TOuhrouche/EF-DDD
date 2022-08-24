using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using EF_DDD.Partnership;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_DDD {
    public class EmployeeContext : Microsoft.EntityFrameworkCore.DbContext {
        private readonly string _connectionString;
        private readonly DbConnection _connection;
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public EmployeeContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString is null");

            _connectionString = connectionString;
        }

        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }

        public EmployeeContext(DbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_connectionString) && _connection == null)
                return;

            if (_connection != null)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connection,
                    options => options.EnableRetryOnFailure().CommandTimeout(300));
            }
            else
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString,
                    options => options.EnableRetryOnFailure().CommandTimeout(300));
            }

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
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
    }
}
