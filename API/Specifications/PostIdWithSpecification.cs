using System;
using API.Entities;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace API.Specifications;

public class PostIdWithSpecification : BaseSpecification<Post>
{
    public PostIdWithSpecification(int id) : base(p => p.Id == id)

    {
        
        AddInclude(p => p.User); // Автор поста
        AddInclude(p => p.Comments); // Коментарі
        AddInclude("Comments.User"); // Автори коментарів — важливо!
        AddInclude(p => p.Location); // Якщо є така навігаційна властивість
    }
}
