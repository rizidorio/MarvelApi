using Marvel.Application.CqrsCharacters.Commads;
using Marvel.Application.CqrsCharacters.Queries;
using Marvel.Application.Dtos;
using Marvel.Application.Interfaces;
using Marvel.Domain.Entities;
using Marvel.Domain.Models;
using MediatR;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Marvel.Application.Services
{
    public class CharactersService : ICharactersService
    {
        private readonly IMediator _mediator;
        private string baseUrl = "https://gateway.marvel.com/v1/public";
        private string apiService = "characters";
        private string privateKey = "817e7302ecfb8cd97608d18c26caab631ca5e6a2";
        private string publicKey = "fd0cb9a2ae3fba6a3f3d383e9a320562";

        public CharactersService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<CharactersDto>> GetCharactersByServer(CharactersFilterModel filter)
        {
            try
            {
                GetCharactersByServerQuery getCharactersByServerQuery = new GetCharactersByServerQuery(filter);

                if (getCharactersByServerQuery == null)
                    throw new Exception("Erro ao listar personagens");

                IEnumerable<Characters> result = await _mediator.Send(getCharactersByServerQuery);

                if (result.Any())
                {
                    return SetCharacters(result);
                }
                return new List<CharactersDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CharactersDto>> GetCharacters(CharactersFilterModel filter)
        {
            try
            {
                GetCharactersFilterQuery getCharactersFilterQuery = new GetCharactersFilterQuery(filter);

                if (getCharactersFilterQuery == null)
                    throw new Exception("Erro ao listar personagens");

                IEnumerable<Characters> result = await _mediator.Send(getCharactersFilterQuery);

                if (result.Any())
                {
                    return SetCharacters(result);
                }
                return new List<CharactersDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CharactersDto>> GetCharacters()
        {
            try
            {
                GetCharactersQuery getCharactersQuery = new GetCharactersQuery();

                if (getCharactersQuery == null)
                    throw new Exception("Erro ao listar personagens");

                IEnumerable<Characters> result = await _mediator.Send(getCharactersQuery);

                if (result.Any())
                {
                    return SetCharacters(result);
                }
                return new List<CharactersDto>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CharactersDto> GetCharactersById(int id)
        {
            try
            {
                GetCharactersByIdQuery getCharactersByIdQuery = new GetCharactersByIdQuery(id);

                if (getCharactersByIdQuery == null)
                    throw new Exception("Erro ao listar personagens");

                Characters result = await _mediator.Send(getCharactersByIdQuery);

                if (result != null)
                {
                    CharactersDto charactersDto = new CharactersDto
                    {
                        Id = result.Id,
                        MarvelId = result.MarvelId,
                        Name = result.Name,
                        Description = result.Description,
                        ImageUrl = result.ImageUrl,
                    };
                    charactersDto.Comics = GetComics(result.MarvelId);
                    charactersDto.Events = GetEvents(result.MarvelId);
                    charactersDto.Series = GetSeries(result.MarvelId);
                    charactersDto.Stories = GetStories(result.MarvelId);
                    return charactersDto;
                }
                return new CharactersDto();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CharactersDto> SetFavorite(SetFavoriteDto setFavoriteDto)
        {
            try
            {
                CharactersUpdateCommand charactersUpdateCommand = new CharactersUpdateCommand
                {
                    Id = setFavoriteDto.Id,
                    IsFavorite = setFavoriteDto.IsFavorite,
                };

                Characters characters = await _mediator.Send(charactersUpdateCommand);

                if (characters != null)
                {
                    return new CharactersDto
                    {
                        Id = characters.Id,
                        Name = characters.Name,
                        Description = characters.Description,
                        ImageUrl = characters.ImageUrl,
                        IsFavorite = characters.IsFavorite,
                        MarvelId = characters.MarvelId
                    };
                }

                return new CharactersDto();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Remove(int id)
        {
            try
            {
                CharactersRemoveCommand charactersRemoveCommand = new CharactersRemoveCommand(id);
                Characters characters = null;

                if (charactersRemoveCommand != null)
                {
                    characters = await _mediator.Send(charactersRemoveCommand);
                }
                return characters != null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private IEnumerable<CharactersDto> SetCharacters(IEnumerable<Characters> result)
        {
            List<CharactersDto> characters = new List<CharactersDto>();
            foreach (Characters character in result)
            {
                CharactersDto charactersDto = new CharactersDto
                {
                    Id = character.Id,
                    MarvelId = character.MarvelId,
                    Name = character.Name,
                    Description = character.Description,
                    ImageUrl = character.ImageUrl,
                    IsFavorite = character.IsFavorite,
                };
                characters.Add(charactersDto);
            }
            return characters;
        }

        private List<ComicsDto> GetComics(int characterId)
        {
            List<ComicsDto> comics = new List<ComicsDto>();
            dynamic? result = GetData(characterId, "comics");

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

        private List<EventsDto> GetEvents(int characterId)
        {
            List<EventsDto> events = new List<EventsDto>();
            dynamic? result = GetData(characterId, "events");
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

        private List<SeriesDto> GetSeries(int characterId)
        {
            List<SeriesDto> series = new List<SeriesDto>();
            dynamic? result = GetData(characterId, "series");
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

        private List<StoriesDto> GetStories(int characterId)
        {
            List<StoriesDto> stories = new List<StoriesDto>();
            dynamic? result = GetData(characterId, "stories");
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

        private dynamic? GetData(int characterId, string dataType)
        {
            string ts = DateTime.Now.Ticks.ToString();
            string hash = GenerateHash(ts);
            string urlKeys = $"ts={ts}&apikey={publicKey}&hash={hash}";

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string characterUrl = $"{baseUrl}/{apiService}/";
            HttpResponseMessage response = client.GetAsync($"{characterUrl}{characterId}/{dataType}?{urlKeys}").Result;
            response.EnsureSuccessStatusCode();
            string content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject(content);
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
