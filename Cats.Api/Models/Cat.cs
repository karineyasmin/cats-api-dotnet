namespace Cats.Api.Models
{
    public class Cat
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // ID automático com GUID
        public string Name { get; set; }
        public string Breed { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}