﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public abstract class QueryableRepositoryBase<TEntity> : QueryableRepositoryBase<TEntity, Guid>, IQueryableRepository<TEntity>
        where TEntity : class, IEntity<Guid>
    {
        
    }

    public abstract class QueryableRepositoryBase<TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>, IQueryableRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual Type ElementType => GetQueryable().ElementType;

        public virtual Expression Expression => GetQueryable().Expression;

        public virtual IQueryProvider Provider => GetQueryable().Provider;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetQueryable().GetEnumerator();
        }

        protected abstract IQueryable<TEntity> GetQueryable();

        public override TEntity Find(TPrimaryKey id)
        {
            return GetQueryable().FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetQueryable().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            Delete(predicate);
            return Task.CompletedTask;
        }
    }
}