using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly ApplicationDbContext _db;
        public CountryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _db.Owners.Where(e => e.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return _db.Owners.Where(c => c.Country.Id == countryId).ToList(); 
        }

        public void Update(Country obj)
        {
            _db.Update(obj);
        }

        
    }
}
