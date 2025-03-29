using System.Text;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using API.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;


namespace API.Services;

public class AuthService : IAuthService
{   
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly string _mail;
    private readonly string _password;

    private readonly IOptions<AuthSettings> _authSettings;

    public AuthService(UserManager<User> userManager, IConfiguration configuration, IOptions<AuthSettings> authSettings)
    {
        _userManager=userManager;
        _configuration=configuration;
        _mail = _configuration["MailSettings:Mail"]!;
        _password = _configuration["MailSettings:Password"]!;
        _authSettings=authSettings;
    } 
    
    

    public string GenerateToken(User? user)
    {
        
        var secret = _configuration["AuthSettings:SecretKey"];
        
        
        byte[] key = Encoding.UTF8.GetBytes(secret);
        var secretKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
        var claims = new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, user!.Id),
            new Claim(JwtRegisteredClaimNames.Email, user!.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, user!.UserName!)
            
        };

        var token = new JwtSecurityToken( 
            issuer: null, audience:null, claims:claims, expires: DateTime.UtcNow.Add(_authSettings.Value.AccessTokenExpiresInDays), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<bool> StoreRefreshToken(string userId, string refreshToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if(user==null) return false;

        int refreshTokenExpiryDays = _configuration.GetValue<int>("AuthSettings:RefreshTokenExpiryTime");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
    
        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<bool> ValidateRefreshToken(string userId, string refreshToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        return user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.UtcNow;
    }


    public async Task<User?> GetUser(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<User?> GetUserByUsername(string userName)
    {
       return await _userManager.FindByNameAsync(userName);
    }

    public string SendEmail(string email, string emailCode)
    {
        StringBuilder emailMessage = new StringBuilder();
        emailMessage.AppendLine("<html>");
        emailMessage.AppendLine("<body>");
        emailMessage. AppendLine($"<p>Dear {email}, </p>");
        emailMessage.AppendLine("<p>Thank you for registering with us. To verify your email please use the following verification code:</p>");
        emailMessage.AppendLine($"<h2>Verification Code: {emailCode}</h2>"); 
        emailMessage.AppendLine("<p>Please enter this code on our website to complete your registration</p>"); 
        emailMessage.AppendLine("<p>If you did not request this, please ignore this email.</p>)");
        emailMessage.AppendLine("<br>");    
        emailMessage.AppendLine("<p>Best regards,</p>"); 
        emailMessage.AppendLine("<p><strong>Travelers</strong></p>");
        emailMessage.AppendLine("</body>");
        emailMessage.AppendLine("</html>");

        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_mail);
        mailMessage.To.Add(email);
        mailMessage.Subject = "Confirmation code";
        mailMessage.Body = emailMessage.ToString();
        mailMessage.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        smtpClient.Port=587;
        smtpClient.UseDefaultCredentials = false;
        
        smtpClient.Credentials = new NetworkCredential(_mail, _password);
        smtpClient.EnableSsl = true;
        smtpClient.DeliveryMethod=SmtpDeliveryMethod.Network;

        try
        {
            smtpClient.Send(mailMessage);
           return "Email Sent Successfully. Check your spam folder.";
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message;
        }
    }
}
