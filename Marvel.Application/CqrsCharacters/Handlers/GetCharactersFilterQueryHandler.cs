using Marvel.Application.CqrsCharacters.Queries;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using MediatR;

namespace Marvel.Application.CqrsCharacters.Handlers
{
    public class GetCharactersFilterQueryHandler : IRequestHandler<GetCharactersFilterQuery, IEnumerable<Characters>>
    {
        private readonly ICharactersRepository _repository;

        public GetCharactersFilterQueryHandler(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Characters>> Handle(GetCharactersFilterQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAll(request.Filter);
        }
    }
}
