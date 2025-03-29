using System;
using API.Models;

namespace API.Entities;

public class Post:BaseEntity
{
   
    public required string Title { get; set; }  
    public required string  PictureUrl { get; set; }
    public string? Description { get; set; }
    public required Location Location { get; set; }
    public int? Like { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}
