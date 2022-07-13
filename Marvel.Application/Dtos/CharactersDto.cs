namespace Marvel.Application.Dtos
{
    public class CharactersDto
    {
        public int Id { get; set; }
        public int MarvelId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
        public List<ComicsDto> Comics { get; set; }
        public List<EventsDto> Events { get; set; }
        public List<SeriesDto> Series { get; set; }
        public List<StoriesDto> Stories { get; set; }
    }

    public class SetFavoriteDto
    {
        public int Id { get; set; }
        public bool IsFavorite { get; set; }
    }
}
