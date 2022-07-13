using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using Marvel.Domain.Models;
using Marvel.Infra.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Marvel.Infra.Repositories
{
    public class CharactersRepository : ICharactersRepository
    {
        private readonly ApplicationDbContext _context;
        private string baseUrl = "https://gateway.marvel.com/v1/public";
        private string apiService = "characters";
        private string privateKey = "817e7302ecfb8cd97608d18c26caab631ca5e6a2";
        private string publicKey = "fd0cb9a2ae3fba6a3f3d383e9a320562";

        public CharactersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Characters>> GetByServer(CharactersFilterModel filter)
        {
            string url = $"{baseUrl}/{apiService}?";

            url = !string.IsNullOrWhiteSpace(filter.Name) ? $"{url}name={filter.Name}&" : url;
            url = !string.IsNullOrWhiteSpace(filter.NameStartsWith) ? $"{url}nameStartsWith={filter.NameStartsWith}&" : url;
            url = filter.Limit.HasValue && filter.Limit.Value > 0 ? $"{url}limit={filter.Limit}&" : url;
            url = filter.OrderByName ? url = $"{url}orderBy=name&" : url;
            
            string ts = DateTime.Now.Ticks.ToString();
            string hash = GenerateHash(ts);
            string urlKeys = $"ts={ts}&apikey={publicKey}&hash={hash}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync($"{url}{urlKeys}").Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            dynamic result = JsonConvert.DeserializeObject(content);
            List<Characters> characters = new List<Characters>();

            foreach (var item in result.data.results)
            {
                int marvelId = Convert.ToInt32(item.id);
                Characters character = await _context.Characters.FirstOrDefaultAsync(x => x.MarvelId.Equals(marvelId));

                if (character != null)
                    continue;

                character = new Characters
                {
                    MarvelId = marvelId,
                    Name = item.name,
                    Description = item.description,
                    ImageUrl = $"{item.thumbnail.path}.{item.thumbnail.extension}",
                };
                characters.Add(character);
            }

            if (characters.Any())
            {
                bool addResult = await AddRange(characters);
                if (addResult)
                    return characters;
            }
            return new List<Characters>();
        }

        public async Task<IEnumerable<Characters>> GetAll(CharactersFilterModel filter)
        {
            IEnumerable<Characters> characters = await _context.Characters.ToListAsync();

            characters = !string.IsNullOrWhiteSpace(filter.Name) ? characters.Where(x => x.Name.Equals(filter.Name)) : characters;
            characters = !string.IsNullOrWhiteSpace(filter.NameStartsWith) ? characters.Where(x => x.Name.StartsWith(filter.NameStartsWith)) : characters;
            characters = filter.IsFavorite.HasValue && filter.IsFavorite.Value ? characters.Where(x => x.IsFavorite) : characters;
            characters = filter.Limit.HasValue && filter.Limit.Value > 0 ? characters.Take(filter.Limit.Value) : characters;
            characters = filter.OrderByName ? characters.OrderBy(x => x.Name) : characters;

            return characters.OrderByDescending(x => x.IsFavorite);
        }

        public async Task<IEnumerable<Characters>> GetAll()
        {
            return await _context.Characters.AsNoTracking().OrderByDescending(x => x.IsFavorite).ToListAsync();
        }

        public async Task<Characters> Get(int id)
        {
            return await _context.Characters.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> AddRange(List<Characters> characters)
        {
            _context.AddRange(characters);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Characters> Update(Characters characters)
        {
            var result = _context.Attach(characters);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Characters> Delete(Characters characters)
        {
            var result = _context.Remove(characters);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        private string GenerateHash(string ts)
        {
            byte[] bytes = Encoding.UTF8.GetBytes($"{ts}{privateKey}{publicKey}");
            var md5Hash = MD5.Create();
            byte[] bytesHash = md5Hash.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
        }
    }
}
