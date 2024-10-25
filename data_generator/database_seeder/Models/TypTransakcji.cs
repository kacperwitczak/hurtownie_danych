using System.ComponentModel.DataAnnotations;

namespace MyConsoleApp.Models
{
    public class TypTransakcji
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Typ { get; set; }

        public ICollection<Transakcje> Transakcje { get; set; }
    }
}
