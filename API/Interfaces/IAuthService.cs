using System;
using API.Models;
using Microsoft.AspNetCore.Identity;

namespace API.Interfaces;

public interface IAuthService
{
    string SendEmail(string email, string emailCode);
    Task<User?> GetUser(string email );

    Task<User?> GetUserByUsername(string userName);

    string GenerateToken(User? user);

    string GenerateRefreshToken();
    Task<bool> StoreRefreshToken(string userId, string refreshToken);
    Task<bool> ValidateRefreshToken(string userId, string refreshToken);

}
