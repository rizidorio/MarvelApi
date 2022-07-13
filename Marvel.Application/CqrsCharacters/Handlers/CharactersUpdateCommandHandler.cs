using Marvel.Application.CqrsCharacters.Commads;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class CharactersUpdateCommandHandler : IRequestHandler<CharactersUpdateCommand, Characters>
    {
        private readonly ICharactersRepository _repository;

        public CharactersUpdateCommandHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Characters> Handle(CharactersUpdateCommand request, CancellationToken cancellationToken)
        {
            IEnumerable<Characters> favorites = _repository.GetAll().Result.Where(x => x.IsFavorite);
            Characters characters = await _repository.Get(request.Id);

            if (characters != null)
            {
                if (request.IsFavorite && favorites.Count() < 5)
                {
                    characters.IsFavorite = true;
                }
                else if (request.IsFavorite && favorites.Count() == 5)
                    throw new ArgumentException("Máximo de personagens favoritos não pode ser maior que 5");
                else if (!request.IsFavorite)
                    characters.IsFavorite = false;
                
                return await _repository.Update(characters);
            }

            throw new ArgumentException("Personagem não enconrado");
        }
    }
}
