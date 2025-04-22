using Cats.Api.Models;
using Cats.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cats.Api.Services
{
    public class CatService
    {
        private readonly ICatRepository _catRepository;

        public CatService(ICatRepository catRepository)
        {
            _catRepository = catRepository;
        }

        public async Task Create(Cat cat)
        {
            await _catRepository.Create(cat);
        }

        public async Task<Cat> GetCatByIdAsync(Guid id)
        {
            return await _catRepository.GetCatByIdAsync(id);
        }

        public async Task<List<Cat>> GetAllCatsAsync()
        {
            return await _catRepository.GetAllCatsAsync();
        }

        public async Task UpdateCat(Cat cat)
        {
            await _catRepository.UpdateCat(cat);
        }

        public async Task DeleteCat(Guid id)
        {
            await _catRepository.DeleteCat(id);
        }
    }
}