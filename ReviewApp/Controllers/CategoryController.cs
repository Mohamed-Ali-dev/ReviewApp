using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Dtos;
using ReviewApp.Models;
using ReviewApp.Repository.IRepository;

namespace ReviewApp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            
            var categories = _mapper.Map<List<CategoryDto>>(_unitOfWork.Category.GetAll());
            if (ModelState.IsValid)
            {
                return Ok(categories);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_unitOfWork.Category.ObjectExist(u => u.Id == categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_unitOfWork.Category.Get(u => u.Id == categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(category);
            }
        }
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategoryId(int categoryId)
        {
            if (!_unitOfWork.Category.ObjectExist(u => u.Id == categoryId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDto>>(_unitOfWork.Category
                .GetPokemonByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemons);
            }
        }
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest(ModelState);
            
            var categoryFromDb = _unitOfWork.Category.Get(U => U.Name.Trim().ToUpper()
            == categoryDto.Name.TrimEnd().ToUpper());
            
            if(categoryFromDb != null)
            {
                ModelState.AddModelError("", "Category already exist");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);
            _unitOfWork.Category.Create(category);
            _unitOfWork.Save();

            return Ok("Successfully created");
        }

    }
}
