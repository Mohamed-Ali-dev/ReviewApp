using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class ReviewerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReviewerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            
            var Reviewers = _mapper.Map<List<ReviewerDto>>(_unitOfWork.Reviewer.GetAll(includeProperties:"Reviews"));
            if (ModelState.IsValid)
            {
                return Ok(Reviewers);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{ReviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int ReviewerId)
        {
            if (!_unitOfWork.Reviewer.ObjectExist(u => u.Id == ReviewerId))
                return NotFound();

            var Reviewer = _mapper.Map<ReviewerDto>(_unitOfWork.Reviewer.Get(u => u.Id == ReviewerId, includeProperties: "Reviews"));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(Reviewer);
            }
        }
   
        [HttpGet("Reviewers/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfAReviewer(int reviewId)
        {
            if (!_unitOfWork.Pokemon.ObjectExist(u => u.Id == reviewId))
                return NotFound();

            var Reviews = _mapper.Map<List<ReviewDto>>(_unitOfWork.Review.GetAll(u => u.ReviewerId == reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(Reviews);
            }

        }
    }
}
