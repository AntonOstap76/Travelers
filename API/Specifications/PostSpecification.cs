using System;
using API.Entities;
using API.Models;

namespace API.Specifications;

public class PostSpecification:BaseSpecification<Post>
{
    public PostSpecification(PostSpecParams specParams)
    {
        ApplyPaging(specParams.PageSize*(specParams.PageIndex-1), specParams.PageSize);
        AddInclude(p => p.User);
        AddInclude(p => p.Comments);
        switch (specParams.Sort)
        {
            case "fromLatest":
                AddOrderBy(x=>x.Created);
                break;
            case "fromNewest":
                AddOrderByDescending(x=>x.Created);
                break;
            case "leastLikes":
                AddOrderBy(x=>x.Like);
                break;
            case "mostLikes":
                AddOrderByDescending(x=>x.Like);
                break;
            default:
                AddOrderBy(x=>x.Title);
                break;
        }
    }

}
