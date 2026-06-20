using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagement.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _set;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken tk = default)
        {
            IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            return await query.ToListAsync(tk);
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken tk = default)
        {
            return await _set.FindAsync(id,tk);
        }

        public async Task<int> AddAsync(TEntity TEntity, CancellationToken tk = default)
        {
            _set.Add(TEntity);
            return await _context.SaveChangesAsync(tk);
        }
        public async Task<int> UpdateAsync(TEntity TEntity, CancellationToken tk = default)
        {
            _set.Update(TEntity);
            return await _context.SaveChangesAsync(tk);
        }
        public async Task<int> DeleteAsync(TEntity TEntity, CancellationToken tk = default)
        {
            _set.Remove(TEntity);
            return await _context.SaveChangesAsync(tk);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken tk = default)
        {
           return await _set.AsNoTracking().AnyAsync(predicate, tk);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = false, CancellationToken tk = default)
        {
            IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, tk);
        }
    }
}
