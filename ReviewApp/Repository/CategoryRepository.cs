using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool CategoryExist(int categoryId)
        {
            return _db.Categories.Any(c => c.Id == categoryId);
        }

        public IEnumerable<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return _db.PokemonCategories.Where(e => e.CategoryId == categoryId)
                .Select(c => c.Pokemon).ToList();
        }

        public void Update(Category category)
        {
          _db.Categories.Update(category);
        }
    }
}
