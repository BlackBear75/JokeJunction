using System;
using System.ComponentModel.DataAnnotations;
using JokeJunction.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace JokeJunction.Domain.ViewModels.Joke
{
    public class JokeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public int Score { get; set; }



        public string TypeJoke { get; set; }
        
      
    }
}