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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = this._mapper.Map<List<CategoryDto>>(this._categoryRepository.GetCategories());

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(categories);

        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!this._categoryRepository.CategoryExists(categoryId))
                return this.NotFound();

            var category = this._mapper.Map<CategoryDto>(this._categoryRepository.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            var pokemons = this._mapper.Map<List<PokemonDto>>(this._categoryRepository.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            return this.Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
            {
                return this.BadRequest(ModelState);
            }

            var category = this._categoryRepository.GetCategories()
                               .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                               .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");

                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var categoryMap = this._mapper.Map<Category>(categoryCreate);

            if (!this._categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");

                return StatusCode(500, ModelState);

            }

            return this.Ok("Succesfully Created");
        }
    }
}
