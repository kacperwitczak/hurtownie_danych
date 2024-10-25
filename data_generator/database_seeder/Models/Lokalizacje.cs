using System.ComponentModel.DataAnnotations;

namespace MyConsoleApp.Models
{
    public class Lokalizacje
    {
        [Key]
        public int ID { get; set; }
        public short Pietro { get; set; }
        public short Rzad { get; set; }
        public short Kolumna { get; set; }

        public ICollection<UstawienieStolu> UstawienieStolu { get; set; }
    }
}
