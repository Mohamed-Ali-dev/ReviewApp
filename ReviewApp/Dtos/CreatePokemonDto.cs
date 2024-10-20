namespace ReviewApp.Dtos
{
    public class CreatePokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public List<int>? OwnerIds { get; set; } // For the PokemonOwner relationship
        public List<int>? CategoryIds { get; set; } // For the PokemonCategory relationship
    }
}
 