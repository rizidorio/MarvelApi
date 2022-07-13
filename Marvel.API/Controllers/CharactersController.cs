using Marvel.Application.Dtos;
using Marvel.Application.Interfaces;
using Marvel.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marvel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharactersService _service;

        public CharactersController(ICharactersService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _service.GetCharactersById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetCharacters());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        [Route("getAllFilter")]
        public async Task<IActionResult> GetAll(CharactersFilterModel filter)
        {
            try
            {
                IEnumerable<CharactersDto> charactersDtos = await _service.GetCharacters(filter);

                if (!charactersDtos.Any())
                    charactersDtos = await _service.GetCharactersByServer(filter);

                return Ok(charactersDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("SetFavorite")]
        public async Task<IActionResult> Update(SetFavoriteDto setFavoriteDto)
        {
            try
            {
                return Ok(await _service.SetFavorite(setFavoriteDto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _service.Remove(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
