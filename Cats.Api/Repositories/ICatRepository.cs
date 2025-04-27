using Cats.Api.Models;

namespace Cats.Api.Repositories
{
    public interface ICatRepository
    {
        Task Create(Cat cat);
        Task<Cat> GetCatByIdAsync(Guid id);
        Task<List<Cat>> GetAllCatsAsync();
        Task UpdateCat(Cat cat);
        Task DeleteCat(Guid id);
    }
}