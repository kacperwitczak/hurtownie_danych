using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Models;

namespace MyConsoleApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string connectionString;

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbSet<Krupierzy> Krupierzy { get; set; }
        public DbSet<TypGry> TypGry { get; set; }
        public DbSet<Stoly> Stoly { get; set; }
        public DbSet<Lokalizacje> Lokalizacje { get; set; }
        public DbSet<UstawienieStolu> UstawienieStolu { get; set; }
        public DbSet<Rozgrywki> Rozgrywki { get; set; }
        public DbSet<TypTransakcji> TypTransakcji { get; set; }
        public DbSet<Transakcje> Transakcje { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
