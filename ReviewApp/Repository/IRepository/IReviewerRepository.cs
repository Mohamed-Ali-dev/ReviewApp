using ReviewApp.Models;

namespace ReviewApp.Repository.IRepository
{
    public interface IReviewerRepository : IRepository<Reviewer>
    {
        void Update(Reviewer obj);

    }
}
