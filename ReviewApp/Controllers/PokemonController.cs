using Microsoft.AspNetCore.Mvc;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class PokemonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PokemonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult GetPokemons()
        {
            var pokemons = _unitOfWork.Pokemon.GetAll(includeProperties:"Reviews");
            if(ModelState.IsValid)
            {
                return Ok(pokemons);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
