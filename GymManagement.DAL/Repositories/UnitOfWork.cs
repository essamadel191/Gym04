using GymManagement.DAL.Context;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;
        private readonly Dictionary<string,object> _repositories = [];

        //Need Database Connection

        public UnitOfWork(GymDbContext dbContext) => _dbContext = dbContext;
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // Check if repo exist or not ??? 
            // IDictionary<>
            var typeName = typeof(TEntity).Name;

            // If Exist in Dictonary => Use it
            // Create Repo => Add Dictionary => Return Repo

            if (_repositories.TryGetValue(typeName, out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(_dbContext);
                _repositories[typeName] = repo;
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken tk)
        {
            return await _dbContext.SaveChangesAsync(tk);
        }
    }
}
