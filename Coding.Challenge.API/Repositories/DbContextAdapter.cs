using Coding.Challenge.Dependencies.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coding.Challenge.API.Repositories
{
    public class DbContextAdapter<TOut, TIn> : IDatabase<TOut, TIn> where TOut : class
    {
        private readonly ApplicationDbContext _context;

        public DbContextAdapter()
        {
            
        }
        public DbContextAdapter(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TOut?> Create(TIn item)
        {
            var createdItem = _context.Add(item as TOut).Entity;
            await _context.SaveChangesAsync();
            return await Task.FromResult(createdItem);
        }

        public async Task<Guid> Delete(Guid id)
        {
            var entity = await _context.FindAsync<TOut>(id);
            if (entity == null) return Guid.Empty;

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<TOut?> Read(Guid id)
        {
            return await _context.FindAsync<TOut>(id);
        }

        public async Task<IEnumerable<TOut?>> ReadAll()
        {
            return await _context.Set<TOut>().ToListAsync();
        }

        public async Task<TOut?> Update(Guid id, TIn item)
        {
            var existingItem = await _context.FindAsync<TOut>(id);
            if (existingItem == null) return default;

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return existingItem;
        }
    }
}
