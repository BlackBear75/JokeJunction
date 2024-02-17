using System;
using JokeJunction.Domain.Enum;

namespace JokeJunction.Domain.Entity
{
    public class Joke
    {
        public int Number { get; set; }
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public double AverageRating { get; set; }

        public double UserVotes { get; set; }

      
        public List<Rating> Ratings { get; set; } = new List<Rating>();

        public TypeJoke TypeJoke { get; set; }



    }
}