using Marvel.Domain.Entities;
using Marvel.Domain.Models;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Queries
{
    public class GetCharactersFilterQuery : IRequest<IEnumerable<Characters>>
    {
        public CharactersFilterModel Filter { get; set; }

        public GetCharactersFilterQuery(CharactersFilterModel filter)
        {
            Filter = filter;
        }
    }
}
