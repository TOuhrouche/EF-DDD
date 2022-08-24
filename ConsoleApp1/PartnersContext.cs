using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using EF_DDD.Partnership;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_DDD {
    public class PartnersContext : Microsoft.EntityFrameworkCore.DbContext {
        private readonly string _connectionString;
        private readonly DbConnection _connection;
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public PartnersContext(string connectionString) : base()
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString is null");

            _connectionString = connectionString;
        }

        public PartnersContext(DbContextOptions<PartnersContext> options) : base(options)
        {
        }

        public PartnersContext(DbConnection dbConnection) : base()
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
            modelBuilder.ApplyConfiguration(new PartnerConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Partner> Partners { get; set; }
    }
}
