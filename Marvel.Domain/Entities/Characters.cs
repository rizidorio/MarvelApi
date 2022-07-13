namespace Marvel.Domain.Entities
{
    public class Characters
    {
        public int Id { get; set; }
        public int MarvelId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
    }
}
