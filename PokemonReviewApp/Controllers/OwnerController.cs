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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper)
        {
            this._ownerRepository = ownerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = this._mapper.Map<List<OwnerDto>>(this._ownerRepository.GetOwners());

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(owners);

        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!this._ownerRepository.OwnerExists(ownerId))
                return this.NotFound();

            var owner = this._mapper.Map<OwnerDto>(this._ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(owner);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!this._ownerRepository.OwnerExists(ownerId))
            {
                return this.NotFound();
            }

            var owner = this._mapper.Map<List<PokemonDto>>(this._ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return this.Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult CreateOwner([FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
            {
                return this.BadRequest(ModelState);
            }

            var owner = this._ownerRepository.GetOwners()
                            .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper())
                            .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "owner already exists");

                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var ownerMap = this._mapper.Map<Owner>(ownerCreate);

            if (!this._ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");

                return StatusCode(500, ModelState);

            }

            return this.Ok("Succesfully Created");
        }
    }
}
