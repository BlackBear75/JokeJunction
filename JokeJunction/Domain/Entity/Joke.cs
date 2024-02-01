using System;
using JokeJunction.Domain.Enum;

namespace JokeJunction.Domain.Entity
{
    public class Joke
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Content { get; set; }
        
        public int Score { get; set; }
        
       
        public TypeJoke TypeJoke { get; set; }


        
    }
}