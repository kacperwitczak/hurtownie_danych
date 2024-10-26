using MyConsoleApp.Data;
using System.Diagnostics;

var DbContext = new ApplicationDbContext();

Console.WriteLine("Seeding database...");

var stopwatch = Stopwatch.StartNew();

var seedService = new DataGeneratorService("data source=laptopkacper;initial catalog=Kasyno_prod;Integrated Security=True;TrustServerCertificate=True;");
await seedService.Seed();

stopwatch.Stop();

Console.WriteLine($"Database seeding completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
