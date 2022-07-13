namespace Marvel.Domain.Models
{
    public class CharactersFilterModel
    {
        public string? Name { get; set; }
        public string? NameStartsWith { get; set; }
        public int? Limit { get; set; }
        public bool OrderByName { get; set; }
        public bool? IsFavorite { get; set; }
    }
}
