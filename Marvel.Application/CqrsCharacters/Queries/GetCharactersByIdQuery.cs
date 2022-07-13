using Marvel.Domain.Entities;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Queries
{
    public class GetCharactersByIdQuery : IRequest<Characters>
    {
        public int Id { get; set; }

        public GetCharactersByIdQuery(int id)
        {
            Id = id;
        }
    }
}
