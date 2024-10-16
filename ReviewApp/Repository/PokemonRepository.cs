using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class PokemonRepository : Repository<Pokemon>, IPokemonRepository
    {
        private readonly ApplicationDbContext _db;
        public PokemonRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _db.Reviews.Where(p => p.Pokemon.Id == pokeId);
            if (review.Count() <= 0)
                return 0;

            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public void Update(Pokemon pokemon)
        {
            _db.Update(pokemon);
        }
    }
}
