using Marvel.Application.CqrsCharacters.Commads;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class CharactersRemoveCommandHandler : IRequestHandler<CharactersRemoveCommand, Characters>
    {
        private readonly ICharactersRepository _repository;

        public CharactersRemoveCommandHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Characters> Handle(CharactersRemoveCommand request, CancellationToken cancellationToken)
        {
            Characters characters = await _repository.Get(request.Id);

            if (characters != null)
                return await _repository.Delete(characters);

            throw new ArgumentException("Personagem não encontrado");
        }
    }
}
