using System.ComponentModel.DataAnnotations;

namespace DogRallyAPI.Models
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}