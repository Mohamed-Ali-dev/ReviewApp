using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        IEnumerable<Review> GetReviewsOfAPokemon(int pokeId);
        void Update(Review obj);

    }
}
