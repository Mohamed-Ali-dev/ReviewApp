using ReviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Dtos
{
    public class OwnerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public int CountryId { get; set; }
    }

}
