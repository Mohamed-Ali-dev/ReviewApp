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


    public class OwnerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OwnerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            
            var owners = _mapper.Map<List<OwnerDto>>(_unitOfWork.Owner.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(owners);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int OwnerId)
        {
            if (!_unitOfWork.Owner.ObjectExist(u => u.Id == OwnerId))
                return NotFound();

            var Owner = _mapper.Map<OwnerDto>(_unitOfWork.Owner.Get(u => u.Id == OwnerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(Owner);
            }
        }
        [HttpGet("pokemon/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_unitOfWork.Owner.ObjectExist(u => u.Id == ownerId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_unitOfWork.Owner
                .GetPokemonByOwner(ownerId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemons);
            }
        }
        [HttpGet("owners/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnerOfAPokemon(int pokeId)
        {
            if (!_unitOfWork.Pokemon.ObjectExist(u => u.Id == pokeId))
                return NotFound();

            var owners =_mapper.Map<List<OwnerDto>>(_unitOfWork.Owner.GetOwnerOfAPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(owners);
            }

        }
        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerDto ownerDto)
        {
            if (ownerDto == null)
                return BadRequest(ModelState);

            var ownerFromDb = _unitOfWork.Owner.Get(U => U.LastName.Trim().ToUpper()
            == ownerDto.LastName.TrimEnd().ToUpper() && U.FirstName.Trim().ToUpper() == ownerDto.FirstName.Trim().ToUpper());

            if (ownerFromDb != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(422, ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var owner = _mapper.Map<Owner>(ownerDto);
            _unitOfWork.Owner.Create(owner);
            _unitOfWork.Save();

            return Ok("Successfully created");
        }
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update([FromBody] OwnerDto ownerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ownerDto.Id == 0)
            {
                return BadRequest("Enter the owner Id");
            }
            var ownerFromDb = _unitOfWork.Owner.Get(c => c.Id == ownerDto.Id);
            if (ownerFromDb == null)
            {
                return NotFound("Owner Not found");
            }
            var ownerFromdatabase = _unitOfWork.Owner.Get(U => U.LastName.Trim().ToUpper()
           == ownerDto.LastName.TrimEnd().ToUpper() && U.FirstName.Trim().ToUpper() == ownerDto.FirstName.Trim().ToUpper());

            if (ownerFromdatabase != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(422, ModelState);
            }
            var owner = _mapper.Map<Owner>(ownerDto);
            _unitOfWork.Owner.Update(owner);
            _unitOfWork.Save();
            return Ok(owner);
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
            if (!_unitOfWork.Owner.ObjectExist(c => c.Id == id))
            {
                return NotFound("Owner Not Found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ownerToBeDeleted = _unitOfWork.Owner.Get(c => c.Id == id);
            _unitOfWork.Owner.Delete(ownerToBeDeleted);
            _unitOfWork.Save();
            return NoContent();

        }
    }
}
