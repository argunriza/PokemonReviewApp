using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            this._countryRepository = countryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = this._mapper.Map<List<CountryDto>>(this._countryRepository.GetCountries());

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(countries);

        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!this._countryRepository.CountryExits(countryId))
                return this.NotFound();

            var country = this._mapper.Map<CountryDto>(this._countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = this._mapper.Map<CountryDto>(this._countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
            {
                return this.BadRequest(ModelState);
            }

            var country = this._countryRepository.GetCountries()
                              .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper())
                              .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "country already exists");

                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var countryMap = this._mapper.Map<Country>(countryCreate);

            if (!this._countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");

                return StatusCode(500, ModelState);

            }

            return this.Ok("Succesfully Created");
        }
    }
}
