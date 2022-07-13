using Marvel.Domain.Entities;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Commads
{
    public class CharactersCommand : IRequest<Characters>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
    }
}
