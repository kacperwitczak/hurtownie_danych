using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyConsoleApp.Models
{
    public class Transakcje
    {
        [Key]
        public int ID { get; set; }
        public decimal Kwota { get; set; }

        public Rozgrywki Rozgrywki { get; set; } // Nawigacyjna właściwość
        public TypTransakcji TypTransakcji { get; set; } // Nawigacyjna właściwość
    }
}
