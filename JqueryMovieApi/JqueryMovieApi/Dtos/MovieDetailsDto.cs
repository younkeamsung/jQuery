using System.ComponentModel.DataAnnotations;

namespace JqueryMovieApi.Dtos
{
    public class MovieDetailsDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; }
    }
}
