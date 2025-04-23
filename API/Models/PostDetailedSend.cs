using System;
using API.Entities;

namespace API.Models;

public class PostDetailedSend
{
     public required string UserName {get; set;}
    public required string Title { get; set; }  
    public required string  PictureUrl { get; set; }
    public string? Description { get; set; }
    public required LocationInput Location { get; set; }
    public int Like { get; set; }
    public List<CommentsSend> Comments { get; set; }

}
