using System;
using API.Models;

namespace API.Entities;

public class Comment:BaseEntity
{
    public User User { get; set; }
    public string UserId { get; set; }
    public int PostId { get; set; }
    public required string Text { get; set; }
    public int Like { get; set; }
}
