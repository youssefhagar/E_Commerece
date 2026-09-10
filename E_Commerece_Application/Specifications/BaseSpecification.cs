using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal abstract class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {


        #region cretria

        public Expression<Func<TEntity, bool>> Cretria { get; private set; }
    
        protected BaseSpecification(Expression<Func<TEntity, bool>> expression = null)
        {
            Cretria = expression;
        }
        #endregion

        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];
        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }
        #endregion

        #region OrederBy
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public void AddOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }


        public Expression<Func<TEntity, object>> OrderByDesc { get; private set; }

    

        public void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        #endregion

        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }


        public void ApplyPaging(int pageSize, int pageIndex)
        {
            IsPagingEnabled = true;
            Skip = pageSize * (pageIndex - 1);
            Take = pageSize;
        }

        #endregion

    }
}
