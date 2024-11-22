
using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IPokemonRepository : IRepository<Pokemon>
    {
        decimal GetPokemonRating(int pokeId);
        void Update(Pokemon pokemon);
    }
}
