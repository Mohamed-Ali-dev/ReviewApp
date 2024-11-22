using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class OwnerRepository : Repository<Owner>, IOwnerRepository
    {
        private readonly ApplicationDbContext _db;
        public OwnerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _db.PokemonOwners.Where(u => u.PokemonId == pokeId)
                .Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _db.PokemonOwners.Where(u => u.OwnerId == ownerId)
                .Select(p => p.Pokemon).ToList();
        }

        public void Update(Owner obj)
        {
            _db.Update(obj);
        }

        
    }
}
