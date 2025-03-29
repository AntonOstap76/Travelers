using System;
using System.Text;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

public static  class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, IConfiguration configuration)
    {

        var authSettings = configuration.GetSection(nameof(AuthSettings)).Get<AuthSettings>();

        serviceCollection.AddAuthentication(options=>
        {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options=>
        {
            options.SaveToken=true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                
                ValidateAudience = false,
                ValidateIssuer  = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey)),
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = true

            };
        });

        return serviceCollection;
    }
}
