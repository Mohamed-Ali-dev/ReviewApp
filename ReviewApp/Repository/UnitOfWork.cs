using ReviewApp.Data;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPokemonRepository Pokemon { get; private set; }
        public ICategoryRepository Category { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Pokemon = new PokemonRepository(_db);
            Category = new CategoryRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
