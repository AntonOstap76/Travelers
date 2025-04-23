using System;

namespace API.Models;

public class CommentsSend
{
    public required string UserName {get; set;}
    public required string Text { get; set; }
    public int Like { get; set; }=0;
}
