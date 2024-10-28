using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]


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
        [HttpPost]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerDto)
        {
            if (reviewerDto == null)
                return BadRequest(ModelState);

            var reviewerFromDb = _unitOfWork.Reviewer.Get(U => U.LastName.Trim().ToUpper()
                  == reviewerDto.LastName.TrimEnd().ToUpper() 
                  && U.FirstName.Trim().ToUpper() == reviewerDto.FirstName.Trim().ToUpper());

            if (reviewerFromDb != null)
            {
                ModelState.AddModelError("", "Reviewer already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewer = _mapper.Map<Reviewer>(reviewerDto);
            _unitOfWork.Reviewer.Create(reviewer);
            _unitOfWork.Save();

            return Ok("Successfully created");
        }
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] ReviewerDto reviewerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reviewerDto.Id == 0)
            {
                return BadRequest("Enter the reviewer Id");
            }
            var reviewerFromDb = _unitOfWork.Reviewer.Get(c => c.Id == reviewerDto.Id);
            if (reviewerFromDb == null)
            {
                return NotFound("Reviewer not found");
            }
            var reviewer = _mapper.Map<Reviewer>(reviewerDto);
            _unitOfWork.Reviewer.Update(reviewer);
            _unitOfWork.Save();
            return Ok(reviewer);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest("id Cannot be zero");
            }
            if (!_unitOfWork.Reviewer.ObjectExist(c => c.Id == id))
            {
                return NotFound("Reviewer Not Found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewerToBeDeleted = _unitOfWork.Reviewer.Get(c => c.Id == id);
            _unitOfWork.Reviewer.Delete(reviewerToBeDeleted);
            _unitOfWork.Save();
            return NoContent();

        }
    }
}
