namespace ReviewApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPokemonRepository Pokemon { get; }
        ICategoryRepository Category { get; }
        ICountryRepository Country { get; }
        IOwnerRepository Owner { get; }
        IReviewRepository Review { get; }
        IReviewerRepository Reviewer { get; }
        IPokemonCategoryRepository PokemonCategory { get; }
        IPokemonOwnerRepository PokemonOwner { get; }

        void Save();
    }
}
