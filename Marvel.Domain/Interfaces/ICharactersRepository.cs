using Marvel.Domain.Entities;
using Marvel.Domain.Models;

namespace Marvel.Domain.Interfaces
{
    public interface ICharactersRepository
    {
        Task<IEnumerable<Characters>> GetByServer(CharactersFilterModel filter);
        Task<IEnumerable<Characters>> GetAll(CharactersFilterModel filter);
        Task<IEnumerable<Characters>> GetAll();
        Task<Characters> Get(int id);
        Task<bool> AddRange(List<Characters> characters);
        Task<Characters> Update(Characters characters);
        Task<Characters> Delete(Characters characters);
    }
}
