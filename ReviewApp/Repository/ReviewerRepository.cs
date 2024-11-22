using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Repository
{
    public class ReviewerRepository : Repository<Reviewer>, IReviewerRepository
    {
        private readonly ApplicationDbContext _db;
        public ReviewerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Reviewer obj)
        {
            _db.Reviewers.Update(obj);

        }
    }
}
