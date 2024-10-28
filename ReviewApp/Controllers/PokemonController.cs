using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]


    public class PokemonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PokemonController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_unitOfWork.Pokemon.GetAll());
            if(ModelState.IsValid)
            {
                return Ok(pokemons);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if(!_unitOfWork.Pokemon.ObjectExist(u => u.Id ==pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_unitOfWork.Pokemon.Get(u => u.Id == pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemon);
            }
        }
        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_unitOfWork.Pokemon.ObjectExist(u => u.Id == pokeId))
                return NotFound();

            var rating = _unitOfWork.Pokemon.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(rating);
            }
        }
        [HttpPost]
        public IActionResult CreatePokemon([FromBody] CreatePokemonDto pokemonDto)
        {
            var pokemonFromDb = _unitOfWork.Pokemon.Get(p => p.Name == pokemonDto.Name);
            if (pokemonFromDb != null)
            {
                ModelState.AddModelError("", "Pokemon already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var pokemon = new Pokemon
            {
                Id = pokemonDto.Id,
                Name = pokemonDto.Name,
                BirthDate = pokemonDto.BirthDate,
            };
            _unitOfWork.Pokemon.Create(pokemon);
            if(pokemonDto.OwnerIds !=null)
            {
                foreach (var ownerID in pokemonDto.OwnerIds)
                {
                    var OwnerFromDb = _unitOfWork.Owner.Get(o => o.Id == ownerID, tracked: true);
                    if (OwnerFromDb != null)
                    {
                        _unitOfWork.PokemonOwner.Create(new PokemonOwner
                        {
                            Owner = OwnerFromDb,
                            Pokemon = pokemon
                        });
                    }
                }
            }    
         
            foreach(var categoryId in pokemonDto.CategoryIds)
            {
                var categoryFromDb = _unitOfWork.Category.Get(c => c.Id == categoryId, tracked:true);
                if(categoryFromDb != null)
                {
                    _unitOfWork.PokemonCategory.Create(new PokemonCategory
                    {
                        Category = categoryFromDb,
                        Pokemon = pokemon
                    });
                }
            }
            _unitOfWork.Save();
            return Ok(new { message = "Pokemon added successfully!" });

        }

        [HttpPut]
        public IActionResult UpdatePokemon([FromBody] CreatePokemonDto pokemonDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonFromdb = _unitOfWork.Pokemon.Get(p => p.Id == pokemonDto.Id,tracked:true);
            if (pokemonFromdb == null)
            {
                return NotFound("Pokemon not found");
            }
            var pokemonFromDb = _unitOfWork.Pokemon.Get(p => p.Name == pokemonDto.Name);
            if (pokemonFromDb != null)
            {
                ModelState.AddModelError("", "Pokemon already exist");
                return StatusCode(422, ModelState);
            }
      
            pokemonFromdb.Name =  pokemonDto.Name;
            pokemonFromdb.BirthDate = pokemonDto.BirthDate;

            if (pokemonDto.OwnerIds != null)
            {
                //clear exiting owners
                var existingPokemonOwners = _unitOfWork.PokemonOwner
                    .GetAll(po => po.PokemonId == pokemonFromdb.Id);
                foreach (var po in existingPokemonOwners)
                {
                    _unitOfWork.PokemonOwner.Delete(po);
                }
                foreach (var ownerID in pokemonDto.OwnerIds)
                {
                    var ownerFromDb = _unitOfWork.Owner.Get(o => o.Id == ownerID, tracked: true);
                    if (ownerFromDb != null)
                    {
                        _unitOfWork.PokemonOwner.Create(new PokemonOwner
                        {
                            Owner = ownerFromDb,
                            Pokemon = pokemonFromdb
                        });
                    }
                }
            }
            if(pokemonDto.CategoryIds != null )
            {
                // Clear existing categories
                var existingPokemonCategories = _unitOfWork.PokemonCategory.GetAll(pc => pc.PokemonId == pokemonFromdb.Id);
                foreach (var pc in existingPokemonCategories)
                {
                    _unitOfWork.PokemonCategory.Delete(pc);
                }

                foreach (var categoryId in pokemonDto.CategoryIds)
                {
                    var categoryFromDb = _unitOfWork.Category.Get(c => c.Id == categoryId, tracked: true);
                    if (categoryFromDb != null)
                    {
                        _unitOfWork.PokemonCategory.Create(new PokemonCategory
                        {
                            Category = categoryFromDb,
                            Pokemon = pokemonFromdb
                        });
                    }
                }
            }
            _unitOfWork.Save();
            return Ok(new { message = "Pokemon updated successfully!" });

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
            if (!_unitOfWork.Pokemon.ObjectExist(c => c.Id == id))
            {
                return NotFound("Pokemon Not Found");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //remove owners of that pokemon 
            var pokemonOwners = _unitOfWork.PokemonOwner.GetAll(po => po.PokemonId == id);
            if(pokemonOwners.Any())
            {
                _unitOfWork.PokemonOwner.DeleteRange(pokemonOwners);
            }
            //remove categories of that pokemon 
            var pokemonCategoriess = _unitOfWork.PokemonCategory.GetAll(po => po.PokemonId == id);
            if (pokemonCategoriess.Any())
            {
                _unitOfWork.PokemonCategory.DeleteRange(pokemonCategoriess);
            }
            var pokemonToBeDeleted = _unitOfWork.Pokemon.Get(c => c.Id == id);
            _unitOfWork.Pokemon.Delete(pokemonToBeDeleted);
            _unitOfWork.Save();
            return NoContent();

        }

    }
}
