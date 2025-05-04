using System;
using System.Security.Claims;
using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentController:BaseApiController
{
    
    private readonly IGenericRepository<Post> _repo;
    private readonly UserManager<User> _userManager;

    public CommentController(IGenericRepository<Post> repo, UserManager<User> userManager)
    {
        _repo = repo;
        _userManager = userManager;
    }

     [Authorize]
    [HttpPost("{postId:int}/comment")]
    public async Task<IActionResult> AddComment(int postId, [FromBody] CommentsDto input)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var comment = new Comment
        {
            Text = input.Text,
            UserId = userId,
            PostId = postId,
            Like = 0
        };

        var post = await _repo.GetByIdAsync(postId);
        if(post == null) return NotFound("Post Not found");

        post.Comments.Add(comment);

         if (!await _repo.SaveAllAsync())
        {
            return BadRequest("Problem saving comment");
        }

        return Ok("Comment added successfully");
    }

    [Authorize]
    [HttpDelete("{postId:int}/comment/{commentId:int}")]
     public async Task<IActionResult>DeleteComment(int postId, int commentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var post = await _repo.GetByIdAsync(postId);
        if(post == null) return NotFound();

        var comment =  post.Comments.FirstOrDefault(c=>c.Id==commentId);
        if(comment == null) return NotFound();

        if (comment.UserId != userId && post.UserId != userId)
        return Forbid("You are not authorized to delete this comment");

        post.Comments.Remove(comment);

        if(await _repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the comment ");
    }


}
