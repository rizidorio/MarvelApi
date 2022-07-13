using Marvel.Domain.Entities;
using Marvel.Domain.Models;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Queries
{
    public class GetCharactersByServerQuery : IRequest<IEnumerable<Characters>>
    {
        public CharactersFilterModel Filter { get; set; }

        public GetCharactersByServerQuery(CharactersFilterModel filter)
        {
            Filter = filter;
        }
    }
}
