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
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            this._pokemonRepository = pokemonRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons =this._mapper.Map<List<PokemonDto>>(this._pokemonRepository.GetPokemons());

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(pokemons);

        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!this._pokemonRepository.PokemonExists(pokeId))
                return this.NotFound();

            var pokemon = this._mapper.Map<PokemonDto>(this._pokemonRepository.GetPokemon(pokeId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!this._pokemonRepository.PokemonExists(pokeId))
                return this.NotFound();

            var rating = this._pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid)
                return this.BadRequest();

            return this.Ok(rating);


        }
    }
}
