namespace JokeJunction.Domain.Entity
{
    public class Rating
    {
        public int Id { get; set; }
        public float Value { get; set; }

        // Зовнішній ключ, що посилається на Id жарту
        public int JokeId { get; set; }
        public Joke Joke { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
