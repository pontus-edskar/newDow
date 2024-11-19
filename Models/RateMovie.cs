using System.ComponentModel.DataAnnotations;

namespace MovieRate.Models;

public class RateMovie
{
        [Required]
        public string? Genre { get; set; }

        [Required]
        public decimal Rate {get; set; }

        [Required]
        public string? Title {get; set; }
}