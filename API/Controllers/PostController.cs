using System;
using System.Security.Claims;
using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostController:BaseApiController
{

    private readonly IPostRepository _postRepository;
    private readonly UserManager<User> _userManager;

     public PostController(IPostRepository postRepository, UserManager<User> userManager)
    {
        _postRepository = postRepository;
        _userManager = userManager;
    }

    
    [HttpGet("posts")]
    public async Task<ActionResult<IReadOnlyList<Post>>> GetPosts()
    {
        var posts = await _postRepository.GetPostsAsync();

        var postDto = posts.Select(p=> new PostsSend
        {
        UserName = p.User?.UserName ?? "Uknown user",
        Title = p.Title,
        PictureUrl = p.PictureUrl,
        Description = p.Description,
        Location = new LocationInput
        {
            Latitude = p.Location.Latitude,
            Longitude = p.Location.Longitude
        },
        Like = p.Like,
        CommentsCount = p.Comments.Count
    }).ToList();

    return Ok(postDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>>GetPost(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post==null) return NotFound();

        var postDto = new PostDetailedSend
        {
        UserName = post.User?.UserName ?? "Uknown user",
        Title = post.Title,
        PictureUrl = post.PictureUrl,
        Description = post.Description,
        Location = new LocationInput
        {
            Latitude = post.Location.Latitude,
            Longitude = post.Location.Longitude
        },
        Like = post.Like,
        Comments = post.Comments.Select(c => new CommentsSend
        {
            Text = c.Text,
            Like = c.Like,
            UserName=c.User?.UserName ?? "Unknown User"
            
        }).ToList()
        };

        return Ok(postDto);
    }

    [Authorize]
    [HttpPost("create-post")]
    public async Task<ActionResult<Post>>CreatePost([FromBody]PostInputInfo postinput)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var post = new Post
        {
            Title = postinput.Title,
            PictureUrl = postinput.PictureUrl,
            Description = postinput.Description,
            UserId = userId,
            Location = new Location
            {
                Latitude = postinput.Location.Latitude,
                Longitude = postinput.Location.Longitude,
                Address = postinput.Location.Address,
                City = postinput.Location.City,
                Country = postinput.Location.Country
            },
            Like = 0,
            Comments = []
           
        };

        _postRepository.AddPost(post);

        if(!await _postRepository.SaveChangesAsync())
        {
           return BadRequest("Problem crewating a post");
        }
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult>DeletePost(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var post = await _postRepository.GetByIdAsync(id);
        if(post == null) return NotFound();

        if(userId != post.UserId){
            return BadRequest("You dont create this post");
        }

        _postRepository.DeletePost(post);

        if(await _postRepository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the post ");
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

        var post = await _postRepository.GetByIdAsync(postId);
        if(post == null) return NotFound("Post Not found");

        post.Comments.Add(comment);

         if (!await _postRepository.SaveChangesAsync())
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

        var post = await _postRepository.GetByIdAsync(postId);
        if(post == null) return NotFound();

        var comment =  post.Comments.FirstOrDefault(c=>c.Id==commentId);
        if(comment == null) return NotFound();

        if (comment.UserId != userId && post.UserId != userId)
        return Forbid("You are not authorized to delete this comment");

        post.Comments.Remove(comment);

        if(await _postRepository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the comment ");
    }



}
