using Bogus;
using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Models;
using System.Collections.Concurrent;

public class DataGeneratorService {
    private readonly ApplicationDbContext _context;
    private readonly int n = 50;

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

        var transakcje = new ConcurrentBag<Transakcje>();

        Parallel.For(0, n * n * n * n * n, _ => {
            transakcje.Add(fakerTransakcje.Generate());
        });

        await _context.Transakcje.AddRangeAsync(transakcje);
        await SaveChanges();
        Console.WriteLine($"Added {transakcje.Count} Transakcje entries.");
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

        var rozgrywki = new ConcurrentBag<Rozgrywki>();

        Parallel.For(0, n * n * n * n, _ => {
            rozgrywki.Add(fakerRozgrywki.Generate());
        });

        await _context.Rozgrywki.AddRangeAsync(rozgrywki);
        await SaveChanges();
        Console.WriteLine($"Added {rozgrywki.Count} Rozgrywki entries.");
    }

    public async Task SeedUstawienieStolu(int n) {
        Console.WriteLine("Seeding UstawienieStolu...");
        var stolyQueue = new ConcurrentQueue<Stoly>(await _context.Stoly.ToListAsync());
        var lokalizacjeQueue = new ConcurrentQueue<Lokalizacje>(await _context.Lokalizacje.ToListAsync());
        var ustawienia = new ConcurrentBag<UstawienieStolu>();

        var fakerUstawienieStolu = new Faker<UstawienieStolu>()
            .RuleFor(u => u.DataStart, f => f.Date.Past(2))
            .RuleFor(u => u.DataKoniec, f => f.Date.Past(1));

        Parallel.For(0, n * n * n, _ => {
            var ustawienieStolu = fakerUstawienieStolu.Generate();
            if (stolyQueue.TryDequeue(out var stol)) {
                ustawienieStolu.Stoly = stol;
            }
            if (lokalizacjeQueue.TryDequeue(out var lokalizacja)) {
                ustawienieStolu.Lokalizacje = lokalizacja;
            }
            ustawienia.Add(ustawienieStolu);
        });

        await _context.UstawienieStolu.AddRangeAsync(ustawienia);
        await SaveChanges();
        Console.WriteLine($"Added {ustawienia.Count} UstawienieStolu entries.");
    }

    public async Task SeedStoly(int n) {
        Console.WriteLine("Seeding Stoly...");
        var typyGier = await _context.TypGry.ToListAsync();
        var fakerStoly = new Faker<Stoly>()
            .RuleFor(s => s.MaksymalnaStawka, f => f.Random.Number(1, 100))
            .RuleFor(s => s.MinimalnaStawka, f => f.Random.Number(100, 1000))
            .RuleFor(s => s.LiczbaMiejsc, f => (short)f.Random.Number(1, 10))
            .RuleFor(s => s.TypGry, f => f.PickRandom(typyGier));

        var stoly = new ConcurrentBag<Stoly>();

        Parallel.For(0, n * n * n, _ => {
            stoly.Add(fakerStoly.Generate());
        });

        await _context.Stoly.AddRangeAsync(stoly);
        await SaveChanges();
        Console.WriteLine($"Added {stoly.Count} Stoly entries.");
    }

    public async Task SeedLokalizacje(int n) {
        Console.WriteLine("Seeding Lokalizacje...");
        var lokalizacje = new ConcurrentBag<Lokalizacje>();

        Parallel.For(0, n, i => {
            Parallel.For(0, n, j => {
                Parallel.For(0, n, k => {
                    lokalizacje.Add(new Lokalizacje {
                        Pietro = (short)i,
                        Rzad = (short)j,
                        Kolumna = (short)k
                    });
                });
            });
        });

        await _context.Lokalizacje.AddRangeAsync(lokalizacje);
        await SaveChanges();
        Console.WriteLine($"Added {lokalizacje.Count} Lokalizacje entries.");
    }

    public async Task SeedKrupierzy(int n) {
        Console.WriteLine("Seeding Krupierzy...");
        var fakerKrupierzy = new Faker<Krupierzy>()
            .RuleFor(k => k.Imie, f => f.Person.FirstName)
            .RuleFor(k => k.Nazwisko, f => f.Person.LastName)
            .RuleFor(k => k.Pesel, f => long.Parse(f.Random.ReplaceNumbers("###########")))
            .RuleFor(k => k.PoczatekPracy, f => f.Date.Past(1));

        var krupierzy = new ConcurrentBag<Krupierzy>();

        Parallel.For(0, n, _ => {
            krupierzy.Add(fakerKrupierzy.Generate());
        });

        await _context.Krupierzy.AddRangeAsync(krupierzy);
        await SaveChanges();
        Console.WriteLine($"Added {krupierzy.Count} Krupierzy entries.");
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
    }

    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }
}
