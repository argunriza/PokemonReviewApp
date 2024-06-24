using Microsoft.EntityFrameworkCore.Diagnostics;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public ICollection<Category> GetCategories()
        {
            return this._dbContext.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return this._dbContext.Categories.Where(e => e.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            return this._dbContext.PokemonCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Pokemon).ToList();
        }

        public bool CategoryExists(int id)
        {
            return this._dbContext.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            this._dbContext.Add(category);

            return this.Save();
        }

        public bool Save()
        {
            var saved = this._dbContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
