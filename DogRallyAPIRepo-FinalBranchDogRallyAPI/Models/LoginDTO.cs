using Microsoft.AspNetCore.Identity;

namespace DogRallyAPI.Models
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
