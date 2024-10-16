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
            throw new NotImplementedException();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            throw new NotImplementedException();
        }

        public void Update(Owner obj)
        {
            _db.Update(obj);
        }

        
    }
}
