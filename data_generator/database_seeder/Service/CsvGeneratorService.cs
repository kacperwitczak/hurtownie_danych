using System.Text;
using System.Collections.Concurrent;
using Bogus;

public static class CsvGeneratorService
{ 
    static string[] games = 
    {
        "Mystic Jungle", "Pharaoh's Fortune", "Golden Spin", "Diamond Rush", "Wild West Slots",
        "Pirate's Treasure", "Fruit Fiesta", "Royal Riches", "Mega Jackpot", "Dragon's Gold",
        "Lucky Sevens", "Space Adventure", "Ocean's Wealth", "Magic Forest", "Casino Royal"
    };
    
    public static void GenerateCsv()
    {
        string filePath = "dane.csv";
        int rowCount = 1000000;
        int threadCount = Environment.ProcessorCount;
        
        var gameFaker = new Faker<Game>()
            .RuleFor(g => g.IdAutomatu, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.IdKartyGracza, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.DataGry, f => f.Date.Recent(30)) // pick dates to you choosing
            .RuleFor(g => g.RodzajGry, f => f.PickRandom(games))
            .RuleFor(g => g.Stawka, f => Math.Round(f.Random.Double(1, 500), 2))
            .RuleFor(g => g.Wygrana, (f, g) => Math.Round(f.Random.Double(0, g.Stawka*1.96), 2));
        
        var outcome = new ConcurrentBag<string>();
        
        Parallel.For(0, rowCount, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, i =>
        {
            var game = gameFaker.Generate();
            string line = $"{game.IdAutomatu};{game.IdKartyGracza};{game.DataGry:yyyy-MM-dd};{game.RodzajGry};{game.Stawka};{game.Wygrana}";
            outcome.Add(line);
        });
        
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine("ID_Automatu;ID_Karty_Gracza;Data_Gry;Rodzaj_Gry;Postawiona_Stawka;Wygrana");
            foreach (var line in outcome)
            {
                writer.WriteLine(line);
            }
        }

        Console.WriteLine("Plik CSV został wygenerowany.");
    }

    public static void GenerateSnapshot1()
    {
        string filePath = "snapshot1.csv";
        int rowCount = 10;
        int threadCount = Environment.ProcessorCount;
        
        int snapshotYear = 2023;
        int snapshotMonth = 2;
        
        int lastDayOfMonth = DateTime.DaysInMonth(snapshotYear, snapshotMonth);
        
        
        var gameFaker = new Faker<Game>()
            .RuleFor(g => g.IdAutomatu, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.IdKartyGracza, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.DataGry, f => f.Date.Recent(30))
            .RuleFor(g => g.RodzajGry, f => f.PickRandom(games))
            .RuleFor(g => g.Stawka, f => Math.Round(f.Random.Double(1, 500), 2))
            .RuleFor(g => g.Wygrana, (f, g) => Math.Round(f.Random.Double(0, g.Stawka*1.96), 2));
        
        var outcome = new ConcurrentBag<string>();
        
        Parallel.For(0, rowCount, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, i =>
        {
            var game = gameFaker.Generate();
            string line = $"{game.IdAutomatu};{game.IdKartyGracza};{game.DataGry:yyyy-MM-dd};{game.RodzajGry};{game.Stawka};{game.Wygrana}";
            outcome.Add(line);
        });
        
        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            writer.WriteLine("ID_Automatu;ID_Karty_Gracza;Data_Gry;Rodzaj_Gry;Postawiona_Stawka;Wygrana");
            foreach (var line in outcome)
            {
                writer.WriteLine(line);
            }
        }
    }

    public static void GenerateSnapshot2()
    {
        string inputFilePath = "snapshot1.csv";
        string outputFilePath = "snapshot2.csv";
        int additionalRowCount = 5;
        int threadCount = Environment.ProcessorCount;

        int snapshotYear = 2023;
        int snapshotMonth = 3;
        
        int lastDayOfMonth = DateTime.DaysInMonth(snapshotYear, snapshotMonth);
        

        var gameFaker = new Faker<Game>()
            .RuleFor(g => g.IdAutomatu, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.IdKartyGracza, f => f.Random.Int(1, 1000))
            .RuleFor(g => g.DataGry, f => f.Date.Between(new DateTime(snapshotYear, snapshotMonth, 1), new DateTime(snapshotYear, snapshotMonth, lastDayOfMonth)))
            .RuleFor(g => g.RodzajGry, f => f.PickRandom(games))
            .RuleFor(g => g.Stawka, f => Math.Round(f.Random.Double(1, 500), 2))
            .RuleFor(g => g.Wygrana, (f, g) => Math.Round(f.Random.Double(0, g.Stawka * 1.96), 2));
        
        var lines = File.ReadAllLines(inputFilePath).ToList();
        
        // update of dimension attribute
        games = games.Select(x => x == "Fruit Fiesta" ? "Fruity Slots" : x).ToArray();

        for (int i = 1; i < lines.Count; i++)
        {
            if (lines[i].Contains("Fruit Fiesta"))
            {
                lines[i] = lines[i].Replace("Fruit Fiesta", "Fruity Slots");
            }
        }
        
        var additionalRows = new ConcurrentBag<string>();
        
        Parallel.For(0, additionalRowCount, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, i =>
        {
            var game = gameFaker.Generate();
            string line = $"{game.IdAutomatu};{game.IdKartyGracza};{game.DataGry:yyyy-MM-dd};{game.RodzajGry};{game.Stawka};{game.Wygrana}";
            additionalRows.Add(line);
        });
        
        lines.AddRange(additionalRows);
        
        using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
        {
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
    }
}

public class Game
{
    public int IdAutomatu { get; set; }
    public int IdKartyGracza { get; set; }
    public DateTime DataGry { get; set; }
    public string RodzajGry { get; set; }
    public double Stawka { get; set; }
    public double Wygrana { get; set; }
}
