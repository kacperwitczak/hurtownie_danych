using MyConsoleApp.Data;
using System.Diagnostics;

Process currentProcess = Process.GetCurrentProcess();
currentProcess.PriorityClass = ProcessPriorityClass.High;

var conn = "Data Source=laptopkacper;Initial Catalog=Kasyno_prod;Integrated Security=True;TrustServerCertificate=True;";

var DbContext = new ApplicationDbContext(conn);

Console.WriteLine("Seeding database...");

var stopwatch = Stopwatch.StartNew();

var seedService = new DataGeneratorService(conn);
await seedService.Seed();

stopwatch.Stop();

Console.WriteLine($"Database seeding completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");

CsvGeneratorService.GenerateCsv();
CsvGeneratorService.GenerateSnapshot1();
CsvGeneratorService.GenerateSnapshot2();