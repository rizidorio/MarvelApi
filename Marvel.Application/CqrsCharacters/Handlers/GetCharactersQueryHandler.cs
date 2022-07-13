using Marvel.Application.CqrsCharacters.Queries;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class GetCharactersQueryHandler : IRequestHandler<GetCharactersQuery, IEnumerable<Characters>>
    {
        private readonly ICharactersRepository _repository;

        public GetCharactersQueryHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Characters>> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll();
        }
    }
}
