using System;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Specifications;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if(spec.Criteria!=null)
        {
            query=query.Where(spec.Criteria);
        }

        foreach (var include in spec.Includes)
        {
            query = query.Include(include);
        }

        foreach (var include in spec.StringInclude)
        {
            query = query.Include(include);
        }

        if(spec.OrderBy!=null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDescending !=null )
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if(spec.IsPagingEnabled)
        {
            query=query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
