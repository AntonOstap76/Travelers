using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using API.Interfaces;
using API.Models;
using API.Services;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace API.Controllers;

public class AuthController(IAuthService authService, UserManager<User> userManager) : BaseApiController
{

   

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserInputModel userInputModel)
    {
        var user = await authService.GetUser(userInputModel.Email);
        if (user != null) return BadRequest("User already exists.");

    
        var newUser = new User()
        {
            UserName = userInputModel.Username,
            Email = userInputModel.Email
        };

        var result = await userManager.CreateAsync(newUser, userInputModel.Password); 

    
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var _user = await authService.GetUser(userInputModel.Email); 

        if (_user == null) return BadRequest("User retrieval failed after creation.");

        var emailCode = await userManager.GenerateEmailConfirmationTokenAsync(_user);

        string sendEmail = authService.SendEmail(_user.Email!, emailCode);
        return Ok(sendEmail);
    }


    

    [HttpPost("confirmation")]
    public async Task<IActionResult>Confirmation([FromBody] UserConfirmEmailInputModel userConfirmEmailInputModel)
    {
        if(string.IsNullOrEmpty(userConfirmEmailInputModel.Email) || userConfirmEmailInputModel.Code<=0){
            return BadRequest("Invalid code");
        }
        var user = await authService.GetUser(userConfirmEmailInputModel.Email);
        if(user==null){
            return BadRequest("Invalid user provided2");
        }

        var confirmEmail = await userManager.ConfirmEmailAsync(user , userConfirmEmailInputModel.Code.ToString()); 
        if(!confirmEmail.Succeeded) {
            return BadRequest("Invalid user provided3");
        }
        else{
            return Ok("Email confirmed successfully, you can proceed to login");
        }
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginInputModel userLoginInputModel)
    {
        if(string.IsNullOrEmpty(userLoginInputModel.UserName)|| string.IsNullOrEmpty(userLoginInputModel.Password)){
            return BadRequest();
        }
        var user = await authService.GetUserByUsername(userLoginInputModel.UserName);

        var isEmailConfirm = await userManager.IsEmailConfirmedAsync(user!);

        if(!isEmailConfirm) return BadRequest("Confirmed email before logging in");
        
        var refreshToken = authService.GenerateRefreshToken();
        await authService.StoreRefreshToken(user.Id, refreshToken);
        
        return Ok(new {AccessToken= authService.GenerateToken(user),
                                            RefreshToken = refreshToken  });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshtokenRequestModel refreshtokenRequestModel )
    {
        if (string.IsNullOrEmpty(refreshtokenRequestModel.RefreshToken)) return BadRequest("Invalid refresh token");

        var user = await authService.GetUserByUsername(refreshtokenRequestModel.UserName);
        if (user == null) return Unauthorized("Invalid user");

        var isValid = await authService.ValidateRefreshToken(user.Id, refreshtokenRequestModel.RefreshToken);
        if (!isValid) return Unauthorized("Invalid refresh token");

        var newAccessToken = authService.GenerateToken(user);
        var newRefreshToken = authService.GenerateRefreshToken();
        await authService.StoreRefreshToken(user.Id, newRefreshToken);

        return Ok(new
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        });
}

    
    //logout just to delete token in frontend


    [HttpGet("protected")]
    [Authorize]
    public string GetMessage()=>"This is wild";


}
