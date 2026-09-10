using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites;
using E_Commerece.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Repository
{
    public class UnitOfWork(StoreDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<String, object> repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if(repositories.TryGetValue(type, out var value))
                return (IGenericRepository<TEntity, TKey>)value;
            else
            {
                var repository = new GenericRepository<TEntity, TKey>(context);
                repositories[type] = repository;
                return repository;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => context.SaveChangesAsync(ct);
    }
}
