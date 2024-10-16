using ReviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int ReviewerId { get; set; }
        public int PokemonId { get; set; }

    }

}
