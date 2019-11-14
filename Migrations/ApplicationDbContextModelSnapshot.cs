﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebClinic.Data;

namespace WebClinic.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebClinic.Models.Users.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("AppUser");
                });

            modelBuilder.Entity("WebClinic.Models.Visit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("PhysicianId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("PhysicianId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("WebClinic.Models.Users.Patient", b =>
                {
                    b.HasBaseType("WebClinic.Models.Users.AppUser");

                    b.Property<string>("IllnessHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecommendedDrugs")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Patient");
                });

            modelBuilder.Entity("WebClinic.Models.Users.Physician", b =>
                {
                    b.HasBaseType("WebClinic.Models.Users.AppUser");

                    b.Property<int>("Specialization")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Physician");
                });

            modelBuilder.Entity("WebClinic.Models.Users.Receptionist", b =>
                {
                    b.HasBaseType("WebClinic.Models.Users.AppUser");

                    b.HasDiscriminator().HasValue("Receptionist");
                });

            modelBuilder.Entity("WebClinic.Models.Visit", b =>
                {
                    b.HasOne("WebClinic.Models.Users.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId");

                    b.HasOne("WebClinic.Models.Users.Physician", "Physician")
                        .WithMany()
                        .HasForeignKey("PhysicianId");
                });
#pragma warning restore 612, 618
        }
    }
}
