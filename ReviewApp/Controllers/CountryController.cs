using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

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
            ICollection<Owner> owmers = _unitOfWork.Country.GetOwnersFromCountry(countryId);
            return Ok(owmers);

        }

    }
}
