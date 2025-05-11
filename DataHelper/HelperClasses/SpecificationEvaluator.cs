using Microsoft.EntityFrameworkCore;

namespace DataHelper.HelperClasses
{
    internal static class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            IQueryable<T> query = inputQuery;

            if (specification != null)
            {
                if (specification.Criteria != null)
                {
                    query = query.Where(specification.Criteria);
                }
                query = specification.Includes.Aggregate(query,
                    (current, include) => current.Include(include));

                query = specification.IncludeStrings.Aggregate(query,
                    (current, include) => current.Include(include));

                if (specification.OrderBys != null)
                {
                    var orderedQuery = query.OrderBy(specification.OrderBys.First());
                    foreach (var orderBy in specification.OrderBys.Skip(1))
                    {
                        orderedQuery = orderedQuery.ThenBy(orderBy);
                    }
                    query = orderedQuery;
                }
                else if (specification.OrderByDescendings != null)
                {
                    var orderedQuery = query.OrderByDescending(specification.OrderByDescendings.First());
                    foreach (var orderByDesc in specification.OrderByDescendings.Skip(1))
                    {
                        orderedQuery = orderedQuery.ThenByDescending(orderByDesc);
                    }
                    query = orderedQuery;
                }

                if (specification.IsPagingEnabled)
                {
                    query = query.Skip(specification.Skip)
                        .Take(specification.Take);
                }
            }
            return query;
        }
    }
}
