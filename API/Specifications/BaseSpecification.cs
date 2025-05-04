using System;
using System.Linq.Expressions;
using API.Interfaces;

namespace API.Specifications;

public class BaseSpecification<T>(Expression<Func<T,bool>>? criteria) : ISpecification<T>
{
    public Expression<Func<T, object>>? OrderBy{get; private set;}
    public Expression<Func<T, object>>? OrderByDescending {get; private set;}
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> StringInclude { get; }=new();
    public Expression<Func<T,bool>>? Criteria=>criteria;

    public int Take {get; private set;}

    public int Skip {get; private set;}

    public bool IsPagingEnabled {get; private set;}

    protected BaseSpecification():this(null){}

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        StringInclude.Add(includeString);
    }

    protected void AddOrderBy(Expression<Func<T,object>> orderByExpression)
    {
        OrderBy=orderByExpression;

    }
    protected void AddOrderByDescending(Expression<Func<T,object>> orderByDescExpression)
    {
        OrderByDescending=orderByDescExpression;
        
    }
    protected void ApplyPaging(int skip, int take)
    {
        Skip=skip;
        Take=take;
        IsPagingEnabled=true;
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
       if(Criteria !=null)
       {
        query = query.Where(Criteria);
       }
       return query;
    }
}
