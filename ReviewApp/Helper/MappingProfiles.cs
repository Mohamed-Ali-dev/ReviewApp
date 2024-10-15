using AutoMapper;
using ReviewApp.Dtos;
using ReviewApp.Models;

namespace ReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
        }
    }
}
