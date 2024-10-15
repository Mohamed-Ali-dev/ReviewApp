namespace ReviewApp.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IPokemonRepository Pokemon { get; }
        void Save();
    }
}
