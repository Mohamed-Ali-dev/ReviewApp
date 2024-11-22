using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int ReviewerId { get; set; }
        [ForeignKey("ReviewerId")]
        public Reviewer Reviewer { get; set; }

        public int PokemonId { get; set; }
        [ForeignKey("PokemonId")]

        public Pokemon Pokemon { get; set; }

    }
}
