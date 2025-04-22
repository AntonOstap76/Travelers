using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class UserInputModel
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]    
    public string Password { get; set; }

}
