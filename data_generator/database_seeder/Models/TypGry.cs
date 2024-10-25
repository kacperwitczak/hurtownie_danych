using System.ComponentModel.DataAnnotations;

namespace MyConsoleApp.Models
{
    public class TypGry
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(100)]
        public string NazwaGry { get; set; }

        public ICollection<Stoly> stoly { get; set; }
    }
}
