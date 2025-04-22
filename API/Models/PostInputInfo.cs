using System;
using API.Entities;

namespace API.Models;

public class PostInputInfo
{
    public required string Title { get; set; }  
    public required string  PictureUrl { get; set; }
    public string? Description { get; set; }
    public required LocationInput Location { get; set; }
   

}
