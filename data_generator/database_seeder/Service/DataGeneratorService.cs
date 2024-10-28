using Bogus;
using Microsoft.EntityFrameworkCore;
using MyConsoleApp.Data;
using MyConsoleApp.Models;
using EFCore.BulkExtensions;
using System.Collections.Concurrent;
using System.Threading;

public class DataGeneratorService
{
    private readonly string _connectionString;
    private readonly int n = 3;

    public DataGeneratorService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task Seed()
    {
        Console.WriteLine("Seeding database...");
        using (var context = new ApplicationDbContext(_connectionString))
        {
            try
            {
                Console.WriteLine("Clearing database...");
                await context.Database.EnsureDeletedAsync();
                Console.WriteLine("Database cleared.");
                Console.WriteLine("Creating db...");
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Db created.");
                await SeedTypGry();
                await SeedTypTransakcji();
                await SeedKrupierzy(n);
                await SeedLokalizacje(n);
                await SeedStoly(n);

                int firstYear = 2023;
                await SeedUstawienieStolu(n, 2023);
                await SeedRozgrywki(2023);
                await SeedTransakcje(2023);

                int secondYear = 2024;
                await SeedUstawienieStolu(n, 2024);
                await SeedRozgrywki(2024);
                await SeedTransakcje(2024);

                Console.WriteLine("Data seeded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }

    public async Task ClearDatabase()
    {
        Console.WriteLine("Clearing database...");
        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Transakcje");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Rozgrywki");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM UstawienieStolu");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Stoly");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Lokalizacje");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM Krupierzy");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM TypTransakcji");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM TypGry");
            Console.WriteLine("Database cleared.");
        }
    }

    public async Task SeedTypGry()
    {
        Console.WriteLine("Seeding TypGry...");
        var typyGier = new List<TypGry>
        {
            new TypGry { NazwaGry = "Poker" },
            new TypGry { NazwaGry = "Blackjack" },
            new TypGry { NazwaGry = "Ruletka" },
            new TypGry { NazwaGry = "Bakarat" },
            new TypGry { NazwaGry = "Keno" },
            new TypGry { NazwaGry = "Kostka" },
            new TypGry { NazwaGry = "Kosci" },
            new TypGry { NazwaGry = "Bingo" }
        };

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(typyGier);
        }

        Console.WriteLine($"Added {typyGier.Count} TypGry entries.");
    }

    public async Task SeedTypTransakcji()
    {
        Console.WriteLine("Seeding TypTransakcji...");
        var typyTransakcji = new List<TypTransakcji>
        {
            new TypTransakcji { Typ = "Wplata" },
            new TypTransakcji { Typ = "Wyplata" }
        };

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(typyTransakcji);
        }

        Console.WriteLine($"Added {typyTransakcji.Count} TypTransakcji entries.");
    }

    public async Task SeedKrupierzy(int n)
    {
        Console.WriteLine("Seeding Krupierzy...");

        var uniquePeselSet = new HashSet<long>(); // Zbiór do przechowywania unikalnych numerów PESEL
        var fakerKrupierzy = new Faker<Krupierzy>()
            .RuleFor(k => k.Imie, f => f.Person.FirstName)
            .RuleFor(k => k.Nazwisko, f => f.Person.LastName)
            .RuleFor(k => k.Pesel,
                f => GenerateUniquePesel(uniquePeselSet)) // Wywołanie metody generującej unikalny PESEL
            .RuleFor(k => k.PoczatekPracy, f =>
            {
                var year = f.Random.Int(2021, 2023); // Wybierz losowy rok od 2021 do 2023
                var month = f.Random.Int(1, 12); // Losowy miesiąc od 1 do 12
                var day = f.Random.Int(1, DateTime.DaysInMonth(year, month)); // Losowy dzień w danym miesiącu
                return DateOnly.FromDateTime(new DateTime(year, month, day)); // Zwróć DateOnly
            });

        var krupierzy = fakerKrupierzy.Generate(n * n * n).ToList();

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(krupierzy);
        }

        Console.WriteLine($"Added {krupierzy.Count} Krupierzy entries.");
    }


    public async Task SeedLokalizacje(int n)
    {
        Console.WriteLine("Seeding Lokalizacje...");
        var lokalizacje = new List<Lokalizacje>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                for (int k = 0; k < n; k++)
                {
                    lokalizacje.Add(new Lokalizacje
                    {
                        Pietro = (short)i,
                        Rzad = (short)j,
                        Kolumna = (short)k
                    });
                }
            }
        }

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(lokalizacje);
        }

        Console.WriteLine($"Added {lokalizacje.Count} Lokalizacje entries.");
    }

    public async Task SeedStoly(int n)
    {
        Console.WriteLine("Seeding Stoly...");
        List<TypGry> typyGier;

        using (var context = new ApplicationDbContext(_connectionString))
        {
            typyGier = await context.TypGry.ToListAsync();
        }

        var fakerStoly = new Faker<Stoly>()
            .RuleFor(s => s.MaksymalnaStawka, f => f.Random.Number(1, 100))
            .RuleFor(s => s.MinimalnaStawka, f => f.Random.Number(100, 1000))
            .RuleFor(s => s.LiczbaMiejsc, f => (short)f.Random.Number(1, 10))
            .RuleFor(s => s.TypGry, f => f.PickRandom(typyGier));

        var stoly = new List<Stoly>();

        stoly.AddRange(fakerStoly.Generate(n * n * n));

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(stoly);
        }

        Console.WriteLine($"Added {stoly.Count} Stoly entries.");
    }

    public async Task SeedUstawienieStolu(int n, int year)
    {
        Console.WriteLine("Seeding UstawienieStolu...");

        List<Stoly> stolyList;
        List<Lokalizacje> lokalizacjeList;

        using (var context = new ApplicationDbContext(_connectionString))
        {
            stolyList = await context.Stoly.ToListAsync();
            lokalizacjeList = await context.Lokalizacje.ToListAsync();
        }

        var ustawienia = new List<UstawienieStolu>();
        var months2024 = GetFirstAndLastDaysOfMonths(year);

        foreach (var month in months2024)
        {
            var offset = month.LastDay.Month - 1;

            for (int i = 0; i < n * n * n; i++)
            {
                var ustawienieStolu = new UstawienieStolu
                {
                    DataStart = month.FirstDay.AddMilliseconds(-month.FirstDay.Millisecond),
                    DataKoniec = month.LastDay.AddMilliseconds(-month.LastDay.Millisecond),
                    Stoly = stolyList[(i + offset) % stolyList.Count],
                    Lokalizacje = lokalizacjeList[(i + offset) % lokalizacjeList.Count]
                };

                ustawienia.Add(ustawienieStolu);
            }
        }

        using (var context = new ApplicationDbContext(_connectionString))
        {
            await context.BulkInsertAsync(ustawienia);
            Console.WriteLine(
                $"Currently in db: {await context.UstawienieStolu.CountAsync()} UstawienieStolu entries.");
        }
    }

    public async Task SeedRozgrywki(int year)
{
    Console.WriteLine("Seeding Rozgrywki...");

    List<UstawienieStolu> ustawieniaStolu;
    List<Krupierzy> krupierzy;

    using (var context = new ApplicationDbContext(_connectionString))
    {
        ustawieniaStolu = await context.UstawienieStolu
            .AsNoTracking()
            .Where(x => x.DataStart.Year >= year && x.DataKoniec.Value.Year <= year)
            .ToListAsync();

        krupierzy = await context.Krupierzy.AsNoTracking().ToListAsync();
    }

    var batchSize = 500; // Reduced batch size for more manageable processing
    var batches = ustawieniaStolu
        .Select((item, index) => new { item, index })
        .GroupBy(x => x.index / batchSize)
        .Select(g => g.Select(x => x.item).ToList())
        .ToList();

    var totalBatches = batches.Count;

    // ConcurrentBag to hold all lists of rozgrywki batches
    var rozgrywkiBag = new ConcurrentBag<List<Rozgrywki>>();

    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    // Concurrent dictionaries to track busy times for dealers and table settings
    var krupierBusy = new ConcurrentDictionary<Krupierzy, List<(DateTime start, DateTime end)>>();
    var ustawienieBusy = new ConcurrentDictionary<UstawienieStolu, List<(DateTime start, DateTime end)>>();

    Parallel.ForEach(batches, options, (batchUstawienia, state, batchIndex) =>
    {
        try
        {
            var rozgrywkiBatch = new List<Rozgrywki>();
            var threadLocalRandom = new Random(); // Create a thread-local Random instance

            foreach (var ustawienie in batchUstawienia)
            {
                var startDate = ustawienie.DataStart;
                var endDate = ustawienie.DataKoniec.Value;

                for (var day = startDate; day <= endDate; day = day.AddDays(1))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Generate start time and duration
                        var godzinaStart = 8 + i; // Start from 8 AM
                        var rozgrywkaStart = new DateTime(day.Year, day.Month, day.Day, godzinaStart, 0, 0);
                        var rozgrywkaEnd = rozgrywkaStart.AddMinutes(threadLocalRandom.Next(1, 60));

                        // Select available dealers
                        var availableKrupierzy = krupierzy
                            .Where(k => k.PoczatekPracy < DateOnly.FromDateTime(rozgrywkaStart))
                            .ToList();

                        foreach (var krupier in availableKrupierzy)
                        {
                            // Check if dealer is busy
                            var isKrupierBusy = krupierBusy.ContainsKey(krupier) &&
                                                krupierBusy[krupier].Any(busy =>
                                                    rozgrywkaStart < busy.end && rozgrywkaEnd > busy.start);

                            // Check if table setting is busy
                            var isUstawienieBusy = ustawienieBusy.ContainsKey(ustawienie) &&
                                                    ustawienieBusy[ustawienie].Any(busy =>
                                                        rozgrywkaStart < busy.end && rozgrywkaEnd > busy.start);

                            if (!isKrupierBusy && !isUstawienieBusy)
                            {
                                // If not busy, create the game
                                var rozgrywka = new Rozgrywki
                                {
                                    DataStart = rozgrywkaStart,
                                    DataKoniec = rozgrywkaEnd,
                                    UstawienieStolu = ustawienie,
                                    Krupier = krupier
                                };

                                rozgrywkiBatch.Add(rozgrywka);

                                // Add busy times to the dictionaries
                                krupierBusy.AddOrUpdate(krupier, new List<(DateTime, DateTime)> { (rozgrywkaStart, rozgrywkaEnd) },
                                    (key, existingList) =>
                                    {
                                        existingList.Add((rozgrywkaStart, rozgrywkaEnd));
                                        return existingList;
                                    });

                                ustawienieBusy.AddOrUpdate(ustawienie, new List<(DateTime, DateTime)> { (rozgrywkaStart, rozgrywkaEnd) },
                                    (key, existingList) =>
                                    {
                                        existingList.Add((rozgrywkaStart, rozgrywkaEnd));
                                        return existingList;
                                    });

                                break; // Found a suitable dealer, exit the loop
                            }
                        }
                    }
                }
            }

            // Add the batch list to the ConcurrentBag
            rozgrywkiBag.Add(rozgrywkiBatch);
            Console.WriteLine($"Processed batch {batchIndex + 1}/{totalBatches} of Rozgrywki.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing batch {batchIndex + 1}: {ex.Message}");
        }
    });

    // Inserting all batches from ConcurrentBag into the database
    using (var context = new ApplicationDbContext(_connectionString))
    {
        for (int batchIndex = 0; batchIndex < rozgrywkiBag.Count; batchIndex++)
        {
            var rozgrywkiBatch = rozgrywkiBag.ElementAt(batchIndex);
            await context.BulkInsertAsync(rozgrywkiBatch);
            Console.WriteLine($"Inserted batch {batchIndex + 1}/{rozgrywkiBag.Count} of Rozgrywki.");
        }

        var rozgrywkiCount = await context.Rozgrywki.CountAsync();
        Console.WriteLine($"Currently in db: {rozgrywkiCount} Rozgrywki entries.");
    }
}




    public async Task SeedTransakcje(int year)
    {
        Console.WriteLine("Seeding Transakcje...");

        List<TypTransakcji> typyTransakcji;
        List<Rozgrywki> rozgrywki;

        // Fetch TypTransakcji and Rozgrywki from the database
        using (var context = new ApplicationDbContext(_connectionString))
        {
            typyTransakcji = await context.TypTransakcji.AsNoTracking().ToListAsync();
            rozgrywki = await context.Rozgrywki
                .AsNoTracking()
                .Where(x => x.DataStart.Year >= year && x.DataKoniec.Year <= year)
                .ToListAsync();
        }

        // Define batch size and split rozgrywki into batches
        var batchSize = 10000;
        var batches = rozgrywki
            .Select((item, index) => new { item, index })
            .GroupBy(x => x.index / batchSize)
            .Select(g => g.Select(x => x.item).ToList())
            .ToList();

        var totalBatches = batches.Count;

        // ConcurrentBag to hold all lists of transakcje batches
        var transakcjeBag = new ConcurrentBag<List<Transakcje>>();

        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        // Parallel processing of each batch
        Parallel.ForEach(batches, options, (batchRozgrywki, state, batchIndex) =>
        {
            try
            {
                var transakcjeBatch = new List<Transakcje>();
                var threadLocalRandom = new Random();

                foreach (var rozgrywka in batchRozgrywki)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var transakcja = new Transakcje
                        {
                            TypTransakcji = typyTransakcji[threadLocalRandom.Next(typyTransakcji.Count)],
                            Rozgrywki = rozgrywka,
                            Kwota = (decimal)(threadLocalRandom.NextDouble() * 999 + 1)
                        };

                        transakcjeBatch.Add(transakcja);
                    }
                }

                // Add the batch list to the ConcurrentBag
                transakcjeBag.Add(transakcjeBatch);
                Console.WriteLine($"Processed batch {batchIndex + 1}/{totalBatches} of Transakcje.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing batch {batchIndex + 1}: {ex.Message}");
            }
        });

        using (var context = new ApplicationDbContext(_connectionString))
        {
            for (int batchIndex = 0; batchIndex < transakcjeBag.Count; batchIndex++)
            {
                var transakcjeBatch = transakcjeBag.ElementAt(batchIndex);
                await context.BulkInsertAsync(transakcjeBatch);
                Console.WriteLine($"Inserted batch {batchIndex + 1}/{totalBatches} of Transakcje.");
            }

            var transakcjeCount = await context.Transakcje.CountAsync();
            Console.WriteLine($"Currently in db: {transakcjeCount} Transakcje entries.");
        }
    }

    public static List<(DateTime FirstDay, DateTime LastDay)> GetFirstAndLastDaysOfMonths(int year)
    {
        var monthDays = new List<(DateTime FirstDay, DateTime LastDay)>();

        for (int month = 1; month <= 12; month++)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            DateTime lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            monthDays.Add((firstDay, lastDay));
        }

        return monthDays;
    }

    private long GenerateUniquePesel(HashSet<long> uniquePeselSet)
    {
        long pesel;

        do
        {
            pesel = long.Parse(new Faker().Random.ReplaceNumbers("###########"));
        } while (!uniquePeselSet.Add(pesel)); // Dodaj do zbioru i sprawdź unikalność

        return pesel;
    }
}