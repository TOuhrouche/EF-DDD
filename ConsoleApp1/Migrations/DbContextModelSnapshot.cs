﻿// <auto-generated />
using System;
using EF_DDD;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EF_DDD.Migrations
{
    [DbContext(typeof(DbContext))]
    partial class DbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EF_DDD.ContractingCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnName("ContractingCompany_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContractingCompany");
                });

            modelBuilder.Entity("EF_DDD.DDD.ContractingCompanyDdd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContractingCompany");
                });

            modelBuilder.Entity("EF_DDD.DDD.ContractorEx", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContractorId")
                        .HasColumnName("ContractorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("EF_DDD.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("Person");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Person");
                });

            modelBuilder.Entity("EF_DDD.Contractor", b =>
                {
                    b.HasBaseType("EF_DDD.Person");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("ContractingCompanyDddId")
                        .HasColumnType("int");

                    b.Property<int>("ContractorId")
                        .HasColumnType("int");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContractingCompanyDddId");

                    b.ToTable("Person1");

                    b.HasDiscriminator().HasValue("Contractor");
                });

            modelBuilder.Entity("EF_DDD.Employee", b =>
                {
                    b.HasBaseType("EF_DDD.Person");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.ToTable("Person2");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("EF_DDD.DDD.ContractingCompanyDdd", b =>
                {
                    b.HasOne("EF_DDD.ContractingCompany", null)
                        .WithOne()
                        .HasForeignKey("EF_DDD.DDD.ContractingCompanyDdd", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EF_DDD.DDD.ContractorEx", b =>
                {
                    b.HasOne("EF_DDD.Person", null)
                        .WithOne()
                        .HasForeignKey("EF_DDD.DDD.ContractorEx", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EF_DDD.Person", b =>
                {
                    b.HasOne("EF_DDD.Person", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");
                });

            modelBuilder.Entity("EF_DDD.Contractor", b =>
                {
                    b.HasOne("EF_DDD.ContractingCompany", "Company")
                        .WithMany("Contractors")
                        .HasForeignKey("CompanyId");

                    b.HasOne("EF_DDD.DDD.ContractingCompanyDdd", null)
                        .WithMany("Contractors")
                        .HasForeignKey("ContractingCompanyDddId");
                });
#pragma warning restore 612, 618
        }
    }
}
