using Bogus;
using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Models;
using System.Collections.Concurrent;

public class DataGeneratorService {
    private readonly ApplicationDbContext _context;
    private readonly int n = 10;
    private readonly int batchSize = 100; // Size of each batch for processing

    public DataGeneratorService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task Seed() {
        Console.WriteLine("Seeding database...");
        if (!_context.Database.EnsureCreated()) {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try {
                await ClearDatabase();
                await SeedTypGry();
                await SeedTypTransakcji();
                await SeedKrupierzy(n);
                await SeedLokalizacje(n);
                await SeedStoly(n);
                await SeedUstawienieStolu(n);
                await SeedRozgrywki(n);
                await SeedTransakcje(n);
                await SaveChanges();
                await transaction.CommitAsync();
                Console.WriteLine("Data seeded successfully.");
            } catch (Exception ex) {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }

    public async Task ClearDatabase() {
        Console.WriteLine("Clearing database...");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Transakcje");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Rozgrywki");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM UstawienieStolu");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Stoly");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Lokalizacje");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM Krupierzy");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM TypTransakcji");
        await _context.Database.ExecuteSqlRawAsync("DELETE FROM TypGry");
        await SaveChanges();
        Console.WriteLine("Database cleared.");
    }

    public async Task SeedTransakcje(int n) {
        Console.WriteLine("Seeding Transakcje...");
        var typyTransakcji = await _context.TypTransakcji.ToListAsync();
        var rozgrywki = await _context.Rozgrywki.ToListAsync();

        var fakerTransakcje = new Faker<Transakcje>()
            .RuleFor(t => t.TypTransakcji, f => f.PickRandom(typyTransakcji))
            .RuleFor(t => t.Rozgrywki, f => f.PickRandom(rozgrywki))
            .RuleFor(t => t.Kwota, f => f.Random.Decimal(1, 1000));

        // Number of transactions to generate
        var totalTransactions = n * n * n * n * n;
        var batches = totalTransactions / batchSize;

        var tasks = new ConcurrentBag<Task>();
        for (int i = 0; i < batches; i++) {
            var batchTransactions = new ConcurrentBag<Transakcje>();
            var start = i * batchSize;
            var end = Math.Min(start + batchSize, totalTransactions);

            // Use Task.Run for each batch
            tasks.Add(Task.Run(() => {
                for (int j = start; j < end; j++) {
                    var transakcja = fakerTransakcje.Generate();
                    batchTransactions.Add(transakcja);
                }
            }));

            // Insert the batch into the database after generating
            if (i % 10 == 0 || i == batches - 1) { // Flush every 10 batches or on last batch
                await Task.WhenAll(tasks);
                await _context.Transakcje.AddRangeAsync(batchTransactions);
                await SaveChanges();
                Console.WriteLine($"Added batch {i + 1}/{batches} of Transakcje entries.");
                tasks.Clear(); // Clear the tasks for the ne    xt batch
            }
        }

        await SaveChanges();
        Console.WriteLine($"Total Transakcje added: {totalTransactions} entries.");
        Console.WriteLine($"Actual count: {_context.Transakcje.Count()}");
    }

    public async Task SeedRozgrywki(int n) {
        Console.WriteLine("Seeding Rozgrywki...");
        var ustawieniaStolu = await _context.UstawienieStolu.ToListAsync();
        var krupierzy = await _context.Krupierzy.ToListAsync();

        var fakerRozgrywki = new Faker<Rozgrywki>()
            .RuleFor(r => r.UstawienieStolu, f => f.PickRandom(ustawieniaStolu))
            .RuleFor(r => r.Krupier, f => f.PickRandom(krupierzy))
            .RuleFor(r => r.DataStart, f => f.Date.Past(2))
            .RuleFor(r => r.DataKoniec, (f, r) => f.Date.Between(r.DataStart, DateTime.Now));

        // Number of games to generate
        var totalRozgrywki = n * n * n * n;
        var batches = totalRozgrywki / batchSize;

        var tasks = new ConcurrentBag<Task>();
        for (int i = 0; i < batches; i++) {
            var batchRozgrywki = new ConcurrentBag<Rozgrywki>();
            var start = i * batchSize;
            var end = Math.Min(start + batchSize, totalRozgrywki);

            // Use Task.Run for each batch
            tasks.Add(Task.Run(() => {
                for (int j = start; j < end; j++) {
                    var gra = fakerRozgrywki.Generate();
                    batchRozgrywki.Add(gra);
                }
            }));

            // Insert the batch into the database after generating
            if (i % 10 == 0 || i == batches - 1) { // Flush every 10 batches or on last batch
                await Task.WhenAll(tasks);
                await _context.Rozgrywki.AddRangeAsync(batchRozgrywki);
                await SaveChanges();
                Console.WriteLine($"Added batch {i + 1}/{batches} of Rozgrywki entries.");
                tasks.Clear(); // Clear the tasks for the next batch
            }
        }

        await SaveChanges();
        Console.WriteLine($"Total Rozgrywki added: {totalRozgrywki} entries.");
        Console.WriteLine($"Actual count: {_context.Rozgrywki.Count()}");
    }

    public async Task SeedUstawienieStolu(int n) {
        Console.WriteLine("Seeding UstawienieStolu...");
        var stolyQueue = new ConcurrentQueue<Stoly>(await _context.Stoly.ToListAsync());
        var lokalizacjeQueue = new ConcurrentQueue<Lokalizacje>(await _context.Lokalizacje.ToListAsync());
        
        var totalUstawienia = n * n * n; // Total number of entries to generate
        var batches = (int)Math.Ceiling((double)totalUstawienia / batchSize); // Number of batches to create
        var tasks = new ConcurrentBag<Task>();

        var fakerUstawienieStolu = new Faker<UstawienieStolu>()
            .RuleFor(u => u.DataStart, f => f.Date.Past(2))
            .RuleFor(u => u.DataKoniec, f => f.Date.Past(1));

        for (int i = 0; i < batches; i++) {
            var batchUstawienia = new ConcurrentBag<UstawienieStolu>();
            var start = i * batchSize;
            var end = Math.Min(start + batchSize, totalUstawienia);
            
            // Generate the batch entries
            tasks.Add(Task.Run(() => {
                for (int j = start; j < end; j++) {
                    var ustawienieStolu = fakerUstawienieStolu.Generate();
                    // Ensure that we dequeue items safely
                    if (stolyQueue.TryDequeue(out var stol)) {
                        ustawienieStolu.Stoly = stol;
                    }
                    if (lokalizacjeQueue.TryDequeue(out var lokalizacja)) {
                        ustawienieStolu.Lokalizacje = lokalizacja;
                    }
                    batchUstawienia.Add(ustawienieStolu);
                }
            }));

            // Insert the batch into the database after generating
            if (i % 10 == 0 || i == batches - 1) { // Flush every 10 batches or on last batch
                await Task.WhenAll(tasks); // Wait for all tasks to complete
                await _context.UstawienieStolu.AddRangeAsync(batchUstawienia);
                await SaveChanges();
                Console.WriteLine($"Added batch {i + 1}/{batches} of UstawienieStolu entries.");
                tasks.Clear(); // Clear the tasks for the next batch
            }
        }

        await SaveChanges();
        Console.WriteLine($"Total UstawienieStolu added: {totalUstawienia} entries.");
        Console.WriteLine($"Actual count: {_context.UstawienieStolu.Count()}");
    }

    public async Task SeedStoly(int n) {
        Console.WriteLine("Seeding Stoly...");
        var typyGier = await _context.TypGry.ToListAsync();
        var totalStoly = n * n * n; // Total number of entries to generate
        var batches = totalStoly / batchSize; // Number of batches to create

        var fakerStoly = new Faker<Stoly>()
            .RuleFor(s => s.MaksymalnaStawka, f => f.Random.Number(1, 100))
            .RuleFor(s => s.MinimalnaStawka, f => f.Random.Number(100, 1000))
            .RuleFor(s => s.LiczbaMiejsc, f => (short)f.Random.Number(1, 10))
            .RuleFor(s => s.TypGry, f => f.PickRandom(typyGier));

        var tasks = new ConcurrentBag<Task>();

        for (int i = 0; i < batches; i++) {
            var batchStoly = new ConcurrentBag<Stoly>();
            var start = i * batchSize;
            var end = Math.Min(start + batchSize, totalStoly);

            // Generate the batch
            tasks.Add(Task.Run(() => {
                for (int j = start; j < end; j++) {
                    var stol = fakerStoly.Generate();
                    batchStoly.Add(stol);
                }
            }));

            // Insert the batch into the database after generating
            if (i % 10 == 0 || i == batches - 1) { // Flush every 10 batches or on last batch
                await Task.WhenAll(tasks);
                await _context.Stoly.AddRangeAsync(batchStoly);
                await SaveChanges();
                Console.WriteLine($"Added batch {i + 1}/{batches} of Stoly entries.");
                tasks.Clear(); // Clear the tasks for the next batch
            }
        }

        await SaveChanges();
        Console.WriteLine($"Total Stoly added: {totalStoly} entries.");
        Thread.Sleep(1000);
        Console.WriteLine($"Actual count: {_context.Stoly.Count()}");
    }

    public async Task SeedLokalizacje(int n) {
        Console.WriteLine("Seeding Lokalizacje...");
        
        var lokalizacje = new ConcurrentBag<Lokalizacje>();
        int totalLokalizacje = n * n * n; // Total number of entries to generate
        int batches = totalLokalizacje / batchSize;

        var tasks = new ConcurrentBag<Task>();

        for (int i = 0; i < batches; i++) {
            var batchLokalizacje = new ConcurrentBag<Lokalizacje>();
            var start = i * batchSize;
            var end = Math.Min(start + batchSize, totalLokalizacje);

            // Generate the batch
            tasks.Add(Task.Run(() => {
                for (int j = start; j < end; j++) {
                    // Calculate the i, j, k coordinates
                    int iCoord = j / (n * n);
                    int jCoord = (j / n) % n;
                    int kCoord = j % n;

                    var lokalizacja = new Lokalizacje {
                        Pietro = (short)iCoord,
                        Rzad = (short)jCoord,
                        Kolumna = (short)kCoord
                    };
                    batchLokalizacje.Add(lokalizacja);
                }
            }));

            // Insert the batch into the database after generating
            if (i % 10 == 0 || i == batches - 1) { // Flush every 10 batches or on last batch
                await Task.WhenAll(tasks);
                await _context.Lokalizacje.AddRangeAsync(batchLokalizacje);
                await SaveChanges();
                Console.WriteLine($"Added batch {i + 1}/{batches} of Lokalizacje entries.");
                tasks.Clear(); // Clear the tasks for the next batch
            }
        }
        
        await SaveChanges();
        Console.WriteLine($"Total Lokalizacje added: {totalLokalizacje} entries.");
        Console.WriteLine($"Actual count: {_context.Lokalizacje.Count()}");
    }

    public async Task SeedKrupierzy(int n) {
        Console.WriteLine("Seeding Krupierzy...");
        var fakerKrupierzy = new Faker<Krupierzy>()
            .RuleFor(k => k.Imie, f => f.Person.FirstName)
            .RuleFor(k => k.Nazwisko, f => f.Person.LastName)
            .RuleFor(k => k.Pesel, f => long.Parse(f.Random.ReplaceNumbers("###########")))
            .RuleFor(k => k.PoczatekPracy, f => f.Date.Past(1));

        var krupierzy = fakerKrupierzy.Generate(n);
        await _context.Krupierzy.AddRangeAsync(krupierzy);
        await SaveChanges();
        Console.WriteLine($"Added {krupierzy.Count} Krupierzy entries.");
        Console.WriteLine($"Actual count: {_context.Krupierzy.Count()}");
    }

    public async Task SeedTypGry() {
        Console.WriteLine("Seeding TypGry...");
        var typyGier = new List<TypGry> {
            new TypGry { NazwaGry = "Poker" },
            new TypGry { NazwaGry = "Blackjack" },
            new TypGry { NazwaGry = "Ruletka" },
            new TypGry { NazwaGry = "Bakarat" },
            new TypGry { NazwaGry = "Keno" },
            new TypGry { NazwaGry = "Kostka" },
            new TypGry { NazwaGry = "Kosci" },
            new TypGry { NazwaGry = "Bingo" }
        };

        await _context.TypGry.AddRangeAsync(typyGier);
        await SaveChanges();
        Console.WriteLine($"Added {typyGier.Count} TypGry entries.");
    }

    public async Task SeedTypTransakcji() {
        Console.WriteLine("Seeding TypTransakcji...");
        var typyTransakcji = new List<TypTransakcji> {
            new TypTransakcji { Typ = "Wplata" },
            new TypTransakcji { Typ = "Wyplata" }
        };

        await _context.TypTransakcji.AddRangeAsync(typyTransakcji);
        await SaveChanges();
        Console.WriteLine($"Added {typyTransakcji.Count} TypTransakcji entries.");
        Console.WriteLine($"Actual count: {_context.TypTransakcji.Count()}");
    }

    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }
}
