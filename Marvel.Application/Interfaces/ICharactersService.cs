using Marvel.Application.Dtos;
using Marvel.Domain.Models;

namespace Marvel.Application.Interfaces
{
    public interface ICharactersService
    {
        Task<IEnumerable<CharactersDto>> GetCharactersByServer(CharactersFilterModel filter);
        Task<IEnumerable<CharactersDto>> GetCharacters(CharactersFilterModel filter);
        Task<IEnumerable<CharactersDto>> GetCharacters();
        Task<CharactersDto> GetCharactersById(int id);
        Task<CharactersDto> SetFavorite(SetFavoriteDto setFavoriteDto);
        Task<bool> Remove(int id);
    }
}
