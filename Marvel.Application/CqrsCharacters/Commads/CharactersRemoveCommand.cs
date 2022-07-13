using Marvel.Domain.Entities;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Commads
{
    public class CharactersRemoveCommand : IRequest<Characters>
    {
        public int Id { get; set; }

        public CharactersRemoveCommand(int id)
        {
            Id = id;
        }
    }
}
