using System;
using System.ComponentModel.DataAnnotations;
using API.Entities;

namespace API.Models;

public class PostInputInfo
{
    [Required]
    public  string Title { get; set; } = string.Empty; 
    [Required]
    public  string  PictureUrl { get; set; }=string.Empty;
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "Location is required!")]
    public LocationInput Location { get; set; }=default!;

}
