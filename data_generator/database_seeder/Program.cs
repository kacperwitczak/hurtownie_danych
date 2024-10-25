using MyConsoleApp.Data;

var DbContext = new ApplicationDbContext();

Console.WriteLine("Seeding database...");

var seedService = new DataGeneratorService(DbContext);
await seedService.Seed();