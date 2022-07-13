using Marvel.Domain.Entities;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Queries
{
    public class GetCharactersQuery : IRequest<IEnumerable<Characters>>
    {
    }
}
