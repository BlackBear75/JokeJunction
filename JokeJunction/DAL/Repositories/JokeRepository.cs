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

        public async Task<Joke> Get(int id)
        {
            return await _db.Jokes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Joke>> Select()
        {
            return await _db.Jokes.ToListAsync();
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

        public async Task<Joke> GetByScore(int score)
        {
            return await _db.Jokes.FirstOrDefaultAsync(x => x.Score == score);
        }

        public async Task<List<Joke>> GetUserJoke(ApplicationUser user)
        {

            return await _db.Jokes
                .Where(j => j.UserId == user.Id)
                .ToListAsync();

           
        }
    }
}