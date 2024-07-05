using Coding.Challenge.Dependencies.Database;
using Coding.Challenge.Dependencies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding.Challenge.Dependencies.Managers
{
    public class ContentsManager : IContentsManager
    {
        private readonly IDatabase<Content?, ContentDto?> _database;
        public ContentsManager(IDatabase<Content?, ContentDto> database)
        {
            _database = database;
        }

        public Task<IEnumerable<Content?>> GetManyContents()
        {
            return _database.ReadAll();
        }

        public Task<Content?> CreateContent(ContentDto content)
        {
            return _database.Create(content);
        }

        public Task<Content?> GetContent(Guid id)
        {
            return _database.Read(id);
        }

        public Task<Content?> UpdateContent(Guid id, ContentDto content)
        {
            return _database.Update(id, content);
        }

        public Task<Guid> DeleteContent(Guid id)
        {
            return _database.Delete(id);
        }
    }
}
