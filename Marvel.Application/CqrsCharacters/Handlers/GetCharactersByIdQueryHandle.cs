using Marvel.Application.CqrsCharacters.Queries;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class GetCharactersByIdQueryHandler : IRequestHandler<GetCharactersByIdQuery, Characters>
    {
        private readonly ICharactersRepository _repository;

        public GetCharactersByIdQueryHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<Characters> Handle(GetCharactersByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.Get(request.Id);
        }
    }
}
