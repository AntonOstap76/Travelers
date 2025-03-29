using System;
using API.Models;

namespace API.Entities;

public class Comment:BaseEntity
{
    //public User User { get; set; }
    public required string Text { get; set; }
}
