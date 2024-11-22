using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class PokemonCategoryRepository : Repository<PokemonCategory>, IPokemonCategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public PokemonCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(PokemonCategory obj)
        {
            _db.Update(obj);
        }

        
    }
}
