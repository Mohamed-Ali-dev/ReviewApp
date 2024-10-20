using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            
            var reviews = _mapper.Map<List<ReviewDto>>(_unitOfWork.Review.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(reviews);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{ReviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int ReviewId)
        {
            if (!_unitOfWork.Review.ObjectExist(u => u.Id == ReviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_unitOfWork.Review.Get(u => u.Id == ReviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(review);
            }
        }
   
        [HttpGet("Reviews/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            if (!_unitOfWork.Pokemon.ObjectExist(u => u.Id == pokeId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(_unitOfWork.Review.GetReviewsOfAPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(reviews);
            }

        }
        [HttpPost]
        public IActionResult CreateReview([FromBody]ReviewDto reviewDto)
        {
            if (reviewDto == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = _mapper.Map<Review>(reviewDto);
            _unitOfWork.Review.Create(review);
            _unitOfWork.Save();

            return Ok("Successfully created");
        }
    }
}
