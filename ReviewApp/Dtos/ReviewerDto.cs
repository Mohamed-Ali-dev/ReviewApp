using ReviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApp.Dtos
{
    public class ReviewerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }

}
