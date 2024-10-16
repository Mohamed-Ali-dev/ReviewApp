namespace ReviewApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPokemonRepository Pokemon { get; }
        ICategoryRepository Category { get; }
        ICountryRepository Country { get; }
        void Save();
    }
}
