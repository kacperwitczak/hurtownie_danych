﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyConsoleApp.Data;

#nullable disable

namespace database_seeder.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241025192013_new-relations")]
    partial class newrelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyConsoleApp.Models.Krupierzy", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("Pesel")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("PoczatekPracy")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Krupierzy");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Lokalizacje", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<short>("Kolumna")
                        .HasColumnType("smallint");

                    b.Property<short>("Pietro")
                        .HasColumnType("smallint");

                    b.Property<short>("Rzad")
                        .HasColumnType("smallint");

                    b.HasKey("ID");

                    b.ToTable("Lokalizacje");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Rozgrywki", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("DataKoniec")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("KrupierID")
                        .HasColumnType("int");

                    b.Property<int>("UstawienieStoluID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("KrupierID");

                    b.HasIndex("UstawienieStoluID");

                    b.ToTable("Rozgrywki");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Stoly", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<short>("LiczbaMiejsc")
                        .HasColumnType("smallint");

                    b.Property<decimal?>("MaksymalnaStawka")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinimalnaStawka")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TypGryID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TypGryID");

                    b.ToTable("Stoly");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Transakcje", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<decimal>("Kwota")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RozgrywkiID")
                        .HasColumnType("int");

                    b.Property<int>("TypTransakcjiID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RozgrywkiID");

                    b.HasIndex("TypTransakcjiID");

                    b.ToTable("Transakcje");
                });

            modelBuilder.Entity("MyConsoleApp.Models.TypGry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("NazwaGry")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.ToTable("TypGry");
                });

            modelBuilder.Entity("MyConsoleApp.Models.TypTransakcji", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Typ")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("TypTransakcji");
                });

            modelBuilder.Entity("MyConsoleApp.Models.UstawienieStolu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime?>("DataKoniec")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("LokalizacjeID")
                        .HasColumnType("int");

                    b.Property<int>("StolyID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("LokalizacjeID");

                    b.HasIndex("StolyID");

                    b.ToTable("UstawienieStolu");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Rozgrywki", b =>
                {
                    b.HasOne("MyConsoleApp.Models.Krupierzy", "Krupier")
                        .WithMany("Rozgrywki")
                        .HasForeignKey("KrupierID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyConsoleApp.Models.UstawienieStolu", "UstawienieStolu")
                        .WithMany("Rozgrywki")
                        .HasForeignKey("UstawienieStoluID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Krupier");

                    b.Navigation("UstawienieStolu");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Stoly", b =>
                {
                    b.HasOne("MyConsoleApp.Models.TypGry", "TypGry")
                        .WithMany("stoly")
                        .HasForeignKey("TypGryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TypGry");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Transakcje", b =>
                {
                    b.HasOne("MyConsoleApp.Models.Rozgrywki", "Rozgrywki")
                        .WithMany("Transakcje")
                        .HasForeignKey("RozgrywkiID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyConsoleApp.Models.TypTransakcji", "TypTransakcji")
                        .WithMany("Transakcje")
                        .HasForeignKey("TypTransakcjiID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rozgrywki");

                    b.Navigation("TypTransakcji");
                });

            modelBuilder.Entity("MyConsoleApp.Models.UstawienieStolu", b =>
                {
                    b.HasOne("MyConsoleApp.Models.Lokalizacje", "Lokalizacje")
                        .WithMany("UstawienieStolu")
                        .HasForeignKey("LokalizacjeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyConsoleApp.Models.Stoly", "Stoly")
                        .WithMany("UstawienieStolu")
                        .HasForeignKey("StolyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lokalizacje");

                    b.Navigation("Stoly");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Krupierzy", b =>
                {
                    b.Navigation("Rozgrywki");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Lokalizacje", b =>
                {
                    b.Navigation("UstawienieStolu");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Rozgrywki", b =>
                {
                    b.Navigation("Transakcje");
                });

            modelBuilder.Entity("MyConsoleApp.Models.Stoly", b =>
                {
                    b.Navigation("UstawienieStolu");
                });

            modelBuilder.Entity("MyConsoleApp.Models.TypGry", b =>
                {
                    b.Navigation("stoly");
                });

            modelBuilder.Entity("MyConsoleApp.Models.TypTransakcji", b =>
                {
                    b.Navigation("Transakcje");
                });

            modelBuilder.Entity("MyConsoleApp.Models.UstawienieStolu", b =>
                {
                    b.Navigation("Rozgrywki");
                });
#pragma warning restore 612, 618
        }
    }
}
