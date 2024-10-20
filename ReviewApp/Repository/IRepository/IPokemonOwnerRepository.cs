using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IPokemonOwnerRepository : IRepository<PokemonOwner>
    {
        void Update(PokemonOwner obj);

    }
}
