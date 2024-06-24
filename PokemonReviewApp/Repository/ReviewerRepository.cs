using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly AppDbContext _dbContext;

        public ReviewerRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return this._dbContext.Reviewers.ToList();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return this._dbContext.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return this._dbContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return this._dbContext.Reviewers.Any(r => r.Id == reviewerId);
        }
    }
}
