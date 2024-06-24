using AutoMapper;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CountryRepository(AppDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public ICollection<Country> GetCountries()
        {
            return this._dbContext.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return this._dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return this._dbContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return this._dbContext.Owners.Where(c => c.Id == countryId).ToList();
        }

        public bool CountryExits(int id)
        {
            return this._dbContext.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country)
        {
            this._dbContext.Add(country);

            return this.Save();
        }

        public bool Save()
        {
            var saved = this._dbContext.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
