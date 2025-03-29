using System;

namespace API.Models;

public class UserConfirmEmailInputModel
{
    public string Email { get; set; }
    public int Code { get; set; }
}
