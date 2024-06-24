using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;

        public ReviewRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public ICollection<Review> GetReviews()
        {
            return this._dbContext.Reviews.ToList();
        }

        public Review GetReview(int reviewId)
        {
            return this._dbContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokeId)
        {
            return this._dbContext.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return this._dbContext.Reviews.Any(r => r.Id == reviewId);
        }
    }
}
