using Cats.Api.Models;
using Cats.Api.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cats.Api.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly AppDbContext _context;

        public CatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(Cat cat)
        {
            _context.Cats.Add(cat);
            await _context.SaveChangesAsync();
        }

        public async Task<Cat> GetCatByIdAsync(Guid id)
        {
            return await _context.Cats.FindAsync(id);
        }

        public async Task<List<Cat>> GetAllCatsAsync()
        {
            return await _context.Cats.ToListAsync();
        }

        public async Task UpdateCat(Cat cat)
        {
            _context.Cats.Update(cat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCat(Guid id)
        {
            var cat = await _context.Cats.FindAsync(id);
            if (cat != null)
            {
                _context.Cats.Remove(cat);
                await _context.SaveChangesAsync();
            }
        }
    }
}