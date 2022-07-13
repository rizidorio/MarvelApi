using Marvel.Application.Dtos;
using Marvel.Application.Interfaces;
using Marvel.Domain.Entities;
using Marvel.Domain.Interfaces;
using Marvel.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Marvel.Application.Services
{
    public class CharactersServerService : ICharactersServerService
    {
        private string baseUrl = "https://gateway.marvel.com/v1/public";
        private string apiService = "characters";
        private string privateKey = "817e7302ecfb8cd97608d18c26caab631ca5e6a2";
        private string publicKey = "fd0cb9a2ae3fba6a3f3d383e9a320562";
        private readonly ICharactersRepository _repository;

        public CharactersServerService(ICharactersRepository repository)
        {
            _repository = repository;
        }

        public List<CharactersDto> GetCharactersByServe(CharactersFilterModel filter)
        {
            try
            {
                List<CharactersDto> characters = new List<CharactersDto>();

                string url = $"{baseUrl}/{apiService}?";

                if (!string.IsNullOrWhiteSpace(filter.Name))
                    url = $"{url}name={filter.Name}&";
                if (!string.IsNullOrWhiteSpace(filter.NameStartsWith))
                    url = $"{url}nameStartsWith={filter.NameStartsWith}&";
                if (filter.Limit.HasValue && filter.Limit.Value > 0)
                    url = $"{url}limit={filter.Limit}&";
                if (filter.OrderByName)
                    url = $"{url}orderBy=name&";

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

                foreach (var item in result.data.results)
                {
                    CharactersDto charactersDto = new CharactersDto
                    {
                        Id = item.id,
                        Name = item.name,
                        Description = item.description,
                        ImageUrl = $"{item.thumbnail.path}.{item.thumbnail.extension}",
                    };
                    charactersDto.Comics = GetComics(charactersDto.Id, urlKeys);
                    charactersDto.Events = GetEvents(charactersDto.Id, urlKeys);
                    charactersDto.Series = GetSeries(charactersDto.Id, urlKeys);
                    charactersDto.Stories = GetStories(charactersDto.Id, urlKeys);
                    characters.Add(charactersDto);
                }

                return characters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GenerateHash(string ts)
        {
            byte[] bytes = Encoding.UTF8.GetBytes($"{ts}{privateKey}{publicKey}");
            var md5Hash = MD5.Create();
            byte[] bytesHash = md5Hash.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
        }

        private List<ComicsDto> GetComics(int characterId, string urlKeys)
        {
            List<ComicsDto> comics = new List<ComicsDto>();
            dynamic? result = GetData(characterId, urlKeys, "comics");

            foreach (var itemComic in result.data.results)
            {
                ComicsDto comicsDto = new ComicsDto
                {
                    Title = itemComic.title,
                    Description = itemComic.description,
                };
                comics.Add(comicsDto);
            }
            return comics;
        }

        private List<EventsDto> GetEvents(int characterId, string urlKeys)
        {
            List<EventsDto> events = new List<EventsDto>();
            dynamic? result = GetData(characterId, urlKeys, "events");
            foreach (var itemEvent in result.data.results)
            {
                EventsDto eventsDto = new EventsDto
                {
                    Title = itemEvent.title,
                    Description = itemEvent.description,
                };
                events.Add(eventsDto);
            }
            return events;
        }

        private List<SeriesDto> GetSeries(int characterId, string urlKeys)
        {
            List<SeriesDto> series = new List<SeriesDto>();
            dynamic? result = GetData(characterId, urlKeys, "series");
            foreach (var itemSerie in result.data.results)
            {
                SeriesDto seriesDto = new SeriesDto
                {
                    Title = itemSerie.title,
                    Description = itemSerie.description,
                    StartYear = itemSerie.startYear,
                    EndYear = itemSerie.endYear,
                };
                series.Add(seriesDto);
            }
            return series;
        }

        private List<StoriesDto> GetStories(int characterId, string urlKeys)
        {
            List<StoriesDto> stories = new List<StoriesDto>();
            dynamic? result = GetData(characterId, urlKeys, "stories");
            foreach (var itemStory in result.data.results)
            {
                StoriesDto storiesDto = new StoriesDto
                {
                    Title = itemStory.title,
                    Description = itemStory.description,
                };
                stories.Add(storiesDto);
            }
            return stories;
        }

        private dynamic? GetData(int characterId, string urlKeys, string dataType)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string characterUrl = $"{baseUrl}/{apiService}/";
            HttpResponseMessage response = client.GetAsync($"{characterUrl}{characterId}/{dataType}?{urlKeys}").Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject(content);
        }
    }
}
