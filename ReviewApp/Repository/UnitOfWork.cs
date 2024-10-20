using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;
using System.Security.Cryptography.X509Certificates;

namespace ReviewApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IPokemonRepository Pokemon { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICountryRepository Country { get; private set; }
        public IOwnerRepository Owner { get; private set; }
        public IReviewRepository Review { get; private set; }
        public IReviewerRepository Reviewer { get; private set; }
        public IPokemonCategoryRepository PokemonCategory { get; private set; }
        public IPokemonOwnerRepository PokemonOwner { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Pokemon = new PokemonRepository(_db);
            Category = new CategoryRepository(_db);
            Country = new CountryRepository(_db);
            Owner = new OwnerRepository(_db);
            Review = new ReviewRepository(_db);
            Reviewer = new ReviewerRepository(_db);
            PokemonCategory = new PokemonCategoryRepository(_db);
            PokemonOwner = new PokemonOwnerRepository(_db);
            
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
