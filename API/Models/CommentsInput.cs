using System;

namespace API.Models;

public class CommentsInput
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string AuthorName { get; set; }
}

