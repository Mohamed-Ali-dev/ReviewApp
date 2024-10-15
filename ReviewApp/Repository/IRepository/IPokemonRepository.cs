
using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IPokemonRepository : IRepository<Pokemon>
    {
        void Update(Pokemon pokemon);
    }
}
