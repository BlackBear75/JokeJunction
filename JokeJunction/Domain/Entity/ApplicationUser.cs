using Microsoft.AspNetCore.Identity;

namespace JokeJunction.Domain.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
    }
}
