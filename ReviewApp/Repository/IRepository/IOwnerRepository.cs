using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        void Update(Owner obj);

    }
}
