using System;
using System.Text.Json.Serialization;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class User:IdentityUser
{

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }

    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
}
