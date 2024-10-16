using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromCountry(int countryId);
        void Update(Country obj);

    }
}
