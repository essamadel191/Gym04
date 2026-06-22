using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken tk = default);

        //GetPlanById
        Task<TEntity> GetByIdAsync(int id, CancellationToken tk = default);

        //Add
        void AddAsync(TEntity TEntity, CancellationToken tk = default);

        //Update
        void UpdateAsync(TEntity TEntity, CancellationToken tk = default);

        //Delete
        void DeleteAsync(TEntity TEntity, CancellationToken tk = default);

        //Check
        Task<bool> AnyAsync(Expression<Func<TEntity,bool>> predicate, CancellationToken tk = default);

        // Check as data or not
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,bool tracking = false ,CancellationToken tk = default);
    }
}
