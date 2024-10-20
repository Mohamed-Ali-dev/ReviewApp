using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IPokemonCategoryRepository : IRepository<PokemonCategory>
    {
        void Update(PokemonCategory obj);

    }
}
