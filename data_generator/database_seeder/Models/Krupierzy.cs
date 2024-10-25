using System.ComponentModel.DataAnnotations;

namespace MyConsoleApp.Models
{
    public class Krupierzy
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Imie { get; set; }
        [Required]
        [StringLength(50)]
        public string? Nazwisko { get; set; }
        [Range(10000000000, 99999999999)]
        public long Pesel { get; set; }
        public DateTime PoczatekPracy { get; set; }

        public ICollection<Rozgrywki> Rozgrywki { get; set; }
    }
}
