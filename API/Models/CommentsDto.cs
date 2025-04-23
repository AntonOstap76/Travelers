using System;

namespace API.Models;

public class CommentsDto
{
    public required string Text { get; set; }
    public int Like { get; set; }=0;
}

