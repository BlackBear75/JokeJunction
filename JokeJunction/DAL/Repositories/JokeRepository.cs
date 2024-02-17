using System.Collections.Generic;
using System.Threading.Tasks;
using JokeJunction.DAL.Interfaces;
using JokeJunction.Domain.Entity;
using JokeJunction.DAL;
using Microsoft.EntityFrameworkCore;

namespace JokeJunction.DAL.Repositories
{
    public class JokeRepository : IJokeRepository
    {
        private readonly ApplicationDbContext _db;

        public JokeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
       

        public async Task<bool> Create(Joke entity)
        {

            await _db.Jokes.AddAsync(entity);
            await _db.SaveChangesAsync();
            

            return true;
        }
        public async Task<bool> GetRatingUser(Joke joke, ApplicationUser currentUser)
        {
            var rating = await _db.Ratings
                .AnyAsync(r => r.JokeId == joke.Id && r.UserId == currentUser.Id);

            return rating;
        }

        public async Task<Joke> Get(int id)
        {
            return await _db.Jokes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Joke>> Select()
        {
            var jokes = await _db.Jokes.ToListAsync();

            foreach (var joke in jokes)
            {
                // Отримуємо всі оцінки для даного жарту
                var ratings = await _db.Ratings.Where(r => r.JokeId == joke.Id).ToListAsync();
                var votes = await  _db.Ratings.CountAsync(r => r.JokeId == joke.Id);

                if (ratings.Any())
                {
                    // Обчислюємо середній бал тільки якщо є оцінки
                    joke.AverageRating = ratings.Average(r => r.Value);
                    joke.UserVotes = votes;
                }
                else
                {
                    // Якщо немає оцінок, середній бал встановлюємо на 0
                    joke.AverageRating = 0;
                    joke.UserVotes = 0;
                }
            }

            // Сортуємо список за середнім балом (в порядку спадання)
            var sortedJokes = jokes.OrderByDescending(j => j.AverageRating).ToList();

            return sortedJokes;
        }

        public async Task<bool> Delete(Joke entity)
        {
            _db.Jokes.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Joke> Update(Joke entity)
        {
            _db.Jokes.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Joke>> GetUserJoke(ApplicationUser user)
        {

            return await _db.Jokes
                .Where(j => j.UserId == user.Id)
                .ToListAsync();

           
        }

        public async Task<int> GetJokeVotes(Joke joke)
        {
            return await _db.Ratings.CountAsync(r => r.JokeId == joke.Id);
        }

        public async Task<bool> RemoveJokeRating(Joke joke, ApplicationUser user)
        {
            
                var rating = await _db.Ratings.FirstOrDefaultAsync(r => r.JokeId == joke.Id && r.UserId == user.Id);

                if (rating != null)
                {
                    _db.Ratings.Remove(rating);

                   
                    await _db.SaveChangesAsync();

                    return true;
                }
                else
                {
                   
                    return false;
                }
         
           
       }

        public async Task<bool> HasUserRatedJoke(Joke joke, ApplicationUser user)
        {
            var rating = await _db.Ratings
         .FirstOrDefaultAsync(r => r.JokeId == joke.Id && r.UserId == user.Id);

            // Перевіряємо, чи користувач оцінив жарт
            if (rating != null && rating.Value > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}