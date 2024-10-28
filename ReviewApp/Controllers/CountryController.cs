using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;
using System.Diagnostics.Metrics;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]


    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            
            var countries = _mapper.Map<List<CountryDto>>(_unitOfWork.Country.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(countries);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_unitOfWork.Country.ObjectExist(u => u.Id == countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_unitOfWork.Country.Get(u => u.Id == countryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(country);
            }
        }
        [HttpGet("/owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_unitOfWork.Country.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(country);
            }
        }
        [HttpGet("/owners/{countryId}")]
        public IActionResult GetOwnersFromCountry(int countryId)
        {
          var owmers = _mapper.Map<List<OwnerDto>>(_unitOfWork.Owner.GetAll(u => u.CountryId == countryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(owmers);
            }

        }
        [HttpPost]
        public IActionResult CreateCountry([FromBody] CountryDto countryDto)
        {
            if (countryDto == null)
                return BadRequest(ModelState);

            var countryFromdB = _unitOfWork.Country.Get(U => U.Name.Trim().ToUpper()
            == countryDto.Name.TrimEnd().ToUpper());

            if (countryFromdB != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = _mapper.Map<Country>(countryDto);
            _unitOfWork.Country.Create(country);
            _unitOfWork.Save();

            return Ok("Successfully created");
        }
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (countryDto.Id == 0)
            {
                return BadRequest("Enter the country Id");
            }
            var countryFromDb = _unitOfWork.Category.Get(c => c.Id == countryDto.Id);
            if (countryFromDb == null)
            {
                return NotFound("there is no country with this id");
            }
            var country = _mapper.Map<Country>(countryDto);
            _unitOfWork.Country.Update(country);
            _unitOfWork.Save();
            return Ok(country);
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
            if (!_unitOfWork.Country.ObjectExist(c => c.Id == id))
            {
                return NotFound("Country Not Found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countryToBeDeleted = _unitOfWork.Country.Get(c => c.Id == id);
            _unitOfWork.Country.Delete(countryToBeDeleted);
            _unitOfWork.Save();
            return NoContent();

        }
    }
}
