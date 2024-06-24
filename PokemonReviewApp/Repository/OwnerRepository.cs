using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _dbContext;

        public OwnerRepository(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public ICollection<Owner> GetOwners()
        {
            return this._dbContext.Owners.ToList();
        }

        public Owner GetOwner(int ownerId)
        {
            return this._dbContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return this._dbContext.PokemonOwners.Where(p => p.PokemonId == pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return this._dbContext.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return this._dbContext.Owners.Any(o => o.Id == ownerId);
        }

        public bool CreateOwner(Owner owner)
        {
            this._dbContext.Add(owner);

            return this.Save();
        }

        public bool Save()
        {
            var saved = this._dbContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
