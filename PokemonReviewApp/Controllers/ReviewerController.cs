using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = this._mapper.Map<List<ReviewerDto>>(this._reviewerRepository.GetReviewers());

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return Ok(reviewers);

        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!this._reviewerRepository.ReviewerExists(reviewerId))
                return this.NotFound();

            var reviewer = this._mapper.Map<ReviewerDto>(this._reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return this.BadRequest(ModelState);

            return this.Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!this._reviewerRepository.ReviewerExists(reviewerId))
            {
                return this.NotFound();

            }

            var reviewer = this._mapper.Map<List<ReviewerDto>>(this._reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            return this.Ok(reviewer);

        }
    }
}
