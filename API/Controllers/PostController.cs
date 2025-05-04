using System;
using System.Security.Claims;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.RequestHelpers;
using API.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostController:BaseApiController
{

    private readonly IGenericRepository<Post> _repo;

     public PostController(IGenericRepository<Post> repo, UserManager<User> userManager)
    {
        _repo = repo;
    }

    
    [HttpGet("posts")]
    public async Task<ActionResult<IReadOnlyList<Post>>> GetPosts([FromQuery]PostSpecParams specParams)
    {  
         //return posts include user
        var spec = new PostSpecification(specParams);
        var posts = await _repo.ListAsync(spec);

        var count = await _repo.CountAsync(spec);
        

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

    var pagination = new Pagination<PostsSend>(specParams.PageIndex, specParams.PageSize, count, postDto);

    return Ok(pagination);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>>GetPost(int id)
    {
        //return post include user+comment include user
        var spec = new PostIdWithSpecification(id);
        var post = await _repo.GetEntityWithSpec(spec);
        
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
            Comments = [],

           
        };
        post.AddEntityCreatedInfo();

        _repo.Add(post);
        

        if(!await _repo.SaveAllAsync())
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

        var post = await _repo.GetByIdAsync(id);
        if(post==null) return NotFound();

        if(userId != post.UserId){
            return BadRequest("You dont create this post");
        }

        _repo.Remove(post);

        if(await _repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting the post ");
    }

// when implementing update
    private bool PostExists(int id)
    {
        return _repo.Exists(id);
    }
}
