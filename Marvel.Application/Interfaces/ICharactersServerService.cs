using Marvel.Application.Dtos;
using Marvel.Domain.Models;

namespace Marvel.Application.Interfaces
{
    public interface ICharactersServerService
    {
        List<CharactersDto> GetCharactersByServe(CharactersFilterModel filter);
    }
}
