using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyConsoleApp.Models
{
    public class Stoly
    {
        [Key]
        public int ID { get; set; }
        public decimal MinimalnaStawka { get; set; }
        public decimal? MaksymalnaStawka { get; set; }
        public short LiczbaMiejsc { get; set; }

        public TypGry TypGry { get; set; }
        public ICollection<UstawienieStolu> UstawienieStolu { get; set; }
    }
}
