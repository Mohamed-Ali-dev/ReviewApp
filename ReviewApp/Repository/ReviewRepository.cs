using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return _db.Reviews.Where(e => e.PokemonId == pokeId).ToList();
        }
        public void Update(Review obj)
        {
            _db.Reviews.Update(obj);

        }
    }
}
