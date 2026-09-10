using E_Commerece.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface ISpecification<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {

        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; }
        public Expression<Func<TEntity, bool>> Cretria { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }

        }
}
