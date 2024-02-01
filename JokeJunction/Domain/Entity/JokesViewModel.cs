namespace JokeJunction.Domain.Entity
{
    public class JokesViewModel
    {
        public List<JokeJunction.Domain.Entity.Joke> Jokes { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
