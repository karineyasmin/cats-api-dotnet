using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Cats.Api.Services;
using Cats.Api.Models;
using Cats.Api.DTOs;

namespace Cats.Api.Controllers
{
    [ApiController]
    [Route("api/v1/cats")]
    public class CatsController : ControllerBase
    {
        private readonly TheCatApiService _theCatApiService;
        private readonly CatService _catService;

        public CatsController(TheCatApiService theCatApiService, CatService catService)
        {
            _theCatApiService = theCatApiService;
            _catService = catService;
        }

        [HttpGet("breeds")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CatBreed>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Gets all cat breeds.", Description = "Returns a list of all cat breeds from TheCatApi.")]
        public async Task<ActionResult<IEnumerable<CatBreed>>> GetAllBreeds()
        {
            var breeds = await _theCatApiService.GetCatBreedsAsync();
            return Ok(breeds);
        }

        [HttpGet("breed")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CatBreed), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Gets a cat breed by ID.", Description = "Returns the cat breed specified by the ID from TheCatApi.")]
        public async Task<ActionResult<CatBreed>> GetBreedById([FromQuery] string id)
        {
            var breed = await _theCatApiService.GetCatBreedByIdAsync(id);
            if (breed == null)
            {
                return NotFound("Breed not found");
            }
            return Ok(breed);
        }

        [HttpGet("breed/image")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Gets the image of a cat breed by ID.", Description = "Returns the image URL of the cat breed specified by the ID from TheCatApi.")]
        public async Task<ActionResult<string>> GetBreedImage([FromQuery] string id)
        {
            var breed = await _theCatApiService.GetCatBreedByIdAsync(id);
            if (breed == null)
            {
                return NotFound("Breed not found");
            }

            var images = await _theCatApiService.GetCatImagesAsync(id);
            var imageUrl = images.FirstOrDefault()?.Url;
            if (string.IsNullOrEmpty(imageUrl))
            {
                return NotFound("Image not found");
            }
            return Ok(imageUrl);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Cat), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Creates a new cat.", Description = "Creates a new cat with the provided details and saves it to the database.")]
        public async Task<ActionResult<Cat>> CreateCat([FromBody] CatDto catDto)
        {
            var breed = await _theCatApiService.GetCatBreedByIdAsync(catDto.BreedId);

            if (breed == null)
            {
                return BadRequest("Breed not found");
            }

            var catImages = await _theCatApiService.GetCatImagesAsync(breed.Id);
            var imageUrl = catImages.FirstOrDefault()?.Url;

            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest("Image not found");
            }

            var cat = new Cat
            {
                Id = Guid.NewGuid(), // Gerar GUID automaticamente
                Name = catDto.Name,
                Breed = breed.Name,
                Description = breed.Description,
                ImageUrl = imageUrl
            };

            await _catService.Create(cat);
            return CreatedAtAction(nameof(GetCat), new { id = cat.Id }, cat);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Cat), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Gets a cat by ID.", Description = "Returns the cat specified by the ID from the database.")]
        public async Task<ActionResult<Cat>> GetCat(Guid id)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            if (cat == null)
            {
                return NotFound("Cat not found");
            }
            return Ok(cat);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Cat>), 200)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Gets all cats.", Description = "Returns a list of all cats from the database.")]
        public async Task<ActionResult<List<Cat>>> GetAllCats()
        {
            var cats = await _catService.GetAllCatsAsync();
            return Ok(cats);
        }

        [HttpPatch("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Updates a cat by ID.", Description = "Updates the data of the cat specified by the ID in the database.")]
        public async Task<IActionResult> UpdateCat(Guid id, [FromBody] CatDto catDto)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            if (cat == null)
            {
                return NotFound("Cat not found");
            }

            var breed = await _theCatApiService.GetCatBreedByIdAsync(catDto.BreedId);
            if (breed == null)
            {
                return BadRequest("Breed not found");
            }

            var catImages = await _theCatApiService.GetCatImagesAsync(breed.Id);
            var imageUrl = catImages.FirstOrDefault()?.Url;

            if (string.IsNullOrEmpty(imageUrl))
            {
                return BadRequest("Image not found");
            }

            cat.Name = catDto.Name;
            cat.Breed = breed.Name;
            cat.Description = breed.Description;
            cat.ImageUrl = imageUrl;

            await _catService.UpdateCat(cat);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [SwaggerOperation(Summary = "Deletes a cat by ID.", Description = "Deletes the cat specified by the ID from the database.")]
        public async Task<IActionResult> DeleteCat(Guid id)
        {
            var cat = await _catService.GetCatByIdAsync(id);
            if (cat == null)
            {
                return NotFound("Cat not found");
            }

            await _catService.DeleteCat(id);
            return NoContent();
        }
    }
}