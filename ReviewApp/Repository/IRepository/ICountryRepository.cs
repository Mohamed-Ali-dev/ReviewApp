using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country GetCountryByOwner(int ownerId);
        void Update(Country obj);

    }
}
