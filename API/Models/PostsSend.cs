using System;

namespace API.Models;

public class PostsSend
{
    public required string UserName {get; set;}
    public required string Title { get; set; }  
    public required string  PictureUrl { get; set; }
    public string? Description { get; set; }
    public required LocationInput Location { get; set; }
    public int Like { get; set; }
    public int Comments { get; set; }
}
