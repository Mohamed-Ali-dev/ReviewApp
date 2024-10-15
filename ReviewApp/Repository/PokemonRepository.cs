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

        public void Update(Pokemon pokemon)
        {
            throw new NotImplementedException();
        }
    }
}
