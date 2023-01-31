﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vehicles.Repository;

#nullable disable

namespace Vehicles.Repository.Migrations
{
    [DbContext(typeof(VehicleDbContext))]
    [Migration("20230131212432_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Vehicles.Repository.Entities.Colour", b =>
                {
                    b.Property<int>("ColourId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ColourId"));

                    b.Property<string>("ColourName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ColourId");

                    b.ToTable("Colour");

                    b.HasData(
                        new
                        {
                            ColourId = 1,
                            ColourName = "Blue"
                        },
                        new
                        {
                            ColourId = 2,
                            ColourName = "Black"
                        },
                        new
                        {
                            ColourId = 3,
                            ColourName = "Red"
                        },
                        new
                        {
                            ColourId = 4,
                            ColourName = "Green"
                        },
                        new
                        {
                            ColourId = 5,
                            ColourName = "Silver"
                        });
                });

            modelBuilder.Entity("Vehicles.Repository.Entities.Make", b =>
                {
                    b.Property<int>("MakeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MakeId"));

                    b.Property<string>("MakeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MakeId");

                    b.ToTable("Make");

                    b.HasData(
                        new
                        {
                            MakeId = 1,
                            MakeName = "Volkswagen"
                        },
                        new
                        {
                            MakeId = 2,
                            MakeName = "Audi"
                        });
                });

            modelBuilder.Entity("Vehicles.Repository.Entities.Model", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ModelId"));

                    b.Property<int>("MakeId")
                        .HasColumnType("int");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ModelId");

                    b.HasIndex("MakeId");

                    b.ToTable("Model");

                    b.HasData(
                        new
                        {
                            ModelId = 1,
                            MakeId = 1,
                            ModelName = "Golf"
                        },
                        new
                        {
                            ModelId = 2,
                            MakeId = 1,
                            ModelName = "Polo"
                        },
                        new
                        {
                            ModelId = 3,
                            MakeId = 2,
                            ModelName = "A4"
                        },
                        new
                        {
                            ModelId = 4,
                            MakeId = 2,
                            ModelName = "A6"
                        });
                });

            modelBuilder.Entity("Vehicles.Repository.Entities.Vehicle", b =>
                {
                    b.Property<int>("VehicleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleId"));

                    b.Property<int>("ColourId")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("VehicleId");

                    b.HasIndex("ColourId");

                    b.HasIndex("ModelId");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("Vehicles.Repository.Entities.Model", b =>
                {
                    b.HasOne("Vehicles.Repository.Entities.Make", "Make")
                        .WithMany()
                        .HasForeignKey("MakeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Make");
                });

            modelBuilder.Entity("Vehicles.Repository.Entities.Vehicle", b =>
                {
                    b.HasOne("Vehicles.Repository.Entities.Colour", "Colour")
                        .WithMany()
                        .HasForeignKey("ColourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Vehicles.Repository.Entities.Model", "Model")
                        .WithMany()
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colour");

                    b.Navigation("Model");
                });
#pragma warning restore 612, 618
        }
    }
}
