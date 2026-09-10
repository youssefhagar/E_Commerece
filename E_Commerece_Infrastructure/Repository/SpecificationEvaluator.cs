using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Repository
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> spec) 
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if (spec.IncludeExpression.Count() >0)
                query = spec.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));


            if (spec.Cretria != null)
                query = query.Where(spec.Cretria);


            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if(spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            return query;
        }
    }
}
