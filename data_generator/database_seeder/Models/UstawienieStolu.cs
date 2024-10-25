using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyConsoleApp.Models
{
    public class UstawienieStolu
    {
        [Key]
        public int ID { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime? DataKoniec { get; set; }
        public Stoly Stoly { get; set; } // Nawigacyjna właściwość
        public Lokalizacje Lokalizacje { get; set; } // Nawigacyjna właściwość

        public ICollection<Rozgrywki> Rozgrywki { get; set; } // Nawigacyjna właściwość
    }
}
