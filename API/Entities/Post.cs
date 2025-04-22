using System;
using System.Text.Json.Serialization;
using API.Models;

namespace API.Entities;

public class Post:BaseEntity
{
    public string UserId { get; set; }
   
    public User User { get; set; }
    public required string Title { get; set; }  
    public required string  PictureUrl { get; set; }
    public string? Description { get; set; }
    public required Location Location { get; set; }
    public int Like { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

}
