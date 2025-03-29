using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class User:IdentityUser
{

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
    
}
