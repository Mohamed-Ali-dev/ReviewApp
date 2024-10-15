using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Pokemon> GetPokemonByCategory(int categoryId);
        bool CategoryExist(int categoryId);

        void Update(Category category);

    }
}
