using Marvel.Application.CqrsCharacters.Queries;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class GetCharactersByServerQueryHandler : IRequestHandler<GetCharactersByServerQuery, IEnumerable<Characters>>
    {
        private readonly ICharactersRepository _repository;

        public GetCharactersByServerQueryHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Characters>> Handle(GetCharactersByServerQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByServer(request.Filter);
        }
    }
}
