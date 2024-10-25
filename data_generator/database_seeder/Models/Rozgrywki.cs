using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyConsoleApp.Models
{
    public class Rozgrywki
    {
        [Key]
        public int ID { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataKoniec { get; set; }

        public Krupierzy Krupier { get; set; }
        public UstawienieStolu UstawienieStolu { get; set; }

        public ICollection<Transakcje> Transakcje { get; set; }
    }
}
