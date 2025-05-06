using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class CommentsDto
{
    [Required]
    public string Text { get; set; }=string.Empty;
    public int Like { get; set; }=0;
}

