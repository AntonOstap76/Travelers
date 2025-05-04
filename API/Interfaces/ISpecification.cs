using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T,bool>>?Criteria{get;}

    //for sorting
    Expression<Func<T, object>>? OrderBy{get;} 
    Expression<Func<T, object>>? OrderByDescending {get;} 

    //for includes and nested Includes
    List<Expression<Func<T, object>>> Includes { get; }  
    List<string> StringInclude { get; }

    //for pagination
    int Take {get;}
    int Skip{get;}
    bool IsPagingEnabled{get;}

    IQueryable<T> ApplyCriteria(IQueryable<T> query);
  
}
