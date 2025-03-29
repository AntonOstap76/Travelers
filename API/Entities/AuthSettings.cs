using System;
using Humanizer;

namespace API.Models;

public class AuthSettings
{
    public TimeSpan AccessTokenExpiresInDays {get;set;}
    public string SecretKey { get; set; }
}
