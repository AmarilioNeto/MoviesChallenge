using Coding.Challenge.API.Models;
using Coding.Challenge.Dependencies.Database;
using Coding.Challenge.Dependencies.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Coding.Challenge.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContentController : Controller
    {
        private readonly IContentsManager _manager;
        private readonly IDatabase<ContentInput, ContentInput> _database;
        private readonly IDatabase<Genrer, Genrer> _databaseGenrer;

        public ContentController(IContentsManager manager, IDatabase<ContentInput, ContentInput> database, IDatabase<Genrer, Genrer> databaseGenrer)
        {
            _manager = manager;
            _database = database;
            _databaseGenrer = databaseGenrer;

        }

        [HttpGet]
        public async Task<IActionResult> GetManyContents()
        {
            var contents = await _manager.GetManyContents().ConfigureAwait(false);

            if (!contents.Any())
                return NotFound();

            return Ok(contents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContent([FromRoute] string id)
        {
            if (!Guid.TryParse(id, out var guid))
            {
                return BadRequest("Invalid UUID format.");
            }

            var content = await _manager.GetContent(guid).ConfigureAwait(false);

            if (content == null)
                return NotFound();

            return Ok(content);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContent([FromBody] ContentInput content)
        {
            var createdContent = await _database.Create(content).ConfigureAwait(false);

            return createdContent == null ? Problem() : Ok(createdContent);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateContent(Guid id, [FromBody] ContentInput content)
        {
            var updatedContent = await _database.Update(id, content).ConfigureAwait(false);

            return updatedContent == null ? NotFound() : Ok(updatedContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContent(Guid id)
        {
            var deletedId = await _database.Delete(id).ConfigureAwait(false);
            return Ok(deletedId);
        }

        [HttpPost("{id}/genre")]
        public async Task<IActionResult> AddGenres(Guid id, [FromBody] IEnumerable<string> genres)
        {
            var contentInput = await _database.Read(id).ConfigureAwait(false);

            if (contentInput == null)
            {
                return NotFound();
            }

            if (contentInput.Genrers == null)
            {
                contentInput.Genrers = new List<Genrer>();
            }

            var contentInputGenrer = await _databaseGenrer.ReadAll().ConfigureAwait(false);

            foreach (var genre in genres)
            {
                if (contentInputGenrer.Any(g => g.Description == genre && g.ContentInputId == id))
                {
                    return BadRequest("Um dos gêneros digitados já existe.");
                }

                contentInput.Genrers.Add(new Genrer { Description = genre, ContentInputId = id });
            }

            var updatedContent = await _database.Update(id, contentInput).ConfigureAwait(false);

            return updatedContent == null ? Problem() : Ok(updatedContent.Genrers.Select(g => g.Description));
        }



        [HttpDelete("{id}/genre")]
        public async Task<IActionResult> RemoveGenres(Guid id)
        {
            var genrer = await _databaseGenrer.Read(id).ConfigureAwait(false);

            if (genrer == null)
            {
                return NotFound();
            }
            else
            {
                await _databaseGenrer.Delete(genrer.Id).ConfigureAwait(false);
            }


            return Ok("Exclusão realizada com sucesso!");
        }

    }
}
