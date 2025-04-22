using Newtonsoft.Json;
using Cats.Api.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Cats.Api.Services
{
    public class TheCatApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<TheCatApiService> _logger;

        public TheCatApiService(HttpClient httpClient, IConfiguration configuration, ILogger<TheCatApiService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TheCatApi:ApiKey"];
            _logger = logger;
        }

        public async Task<List<CatBreed>> GetCatBreedsAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.thecatapi.com/v1/breeds");
                request.Headers.Add("x-api-key", _apiKey);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CatBreed>>(content);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching cat breeds from TheCatApi");
                throw new Exception("Error fetching cat breeds from TheCatApi");
            }
        }

        public async Task<CatBreed> GetCatBreedByIdAsync(string id)
        {
            try
            {
                var breeds = await GetCatBreedsAsync();
                return breeds.FirstOrDefault(b => b.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cat breed by ID from TheCatApi");
                throw new Exception("Error fetching cat breed by ID from TheCatApi");
            }
        }

        public async Task<List<CatImage>> GetCatImagesAsync(string breedId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.thecatapi.com/v1/images/search?breed_id={breedId}");
                request.Headers.Add("x-api-key", _apiKey);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CatImage>>(content);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching cat images from TheCatApi");
                throw new Exception("Error fetching cat images from TheCatApi");
            }
        }
    }
}