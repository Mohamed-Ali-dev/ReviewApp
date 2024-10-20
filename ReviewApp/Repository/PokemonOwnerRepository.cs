using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class PokemonOwnerRepository : Repository<PokemonOwner>, IPokemonOwnerRepository
    {
        private readonly ApplicationDbContext _db;
        public PokemonOwnerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(PokemonOwner obj)
        {
            _db.Update(obj);
        }

        
    }
}
