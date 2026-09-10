using E_Commerece.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IReadOnlyList<TEntity>?> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<TEntity>?> GetAllAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct);
        Task<int> GetProductsCountAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct);
        Task<TEntity?> GetByIdASync(TKey id,CancellationToken ct);
        Task<TEntity?> GetByIdASync(ISpecification<TEntity, TKey> spec, CancellationToken ct);
        Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity, TKey> specification, CancellationToken ct = default);


    }
}
