using ReviewApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        [Required]
        public int ReviewerId { get; set; }
        [Required]

        public int PokemonId { get; set; }

    }

}
