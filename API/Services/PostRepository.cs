using System;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class PostRepository : IPostRepository
{
    private readonly StoreContext _context;

    public PostRepository(StoreContext context)
    {
        _context = context;
    }
    public void AddPost(Post post)
    {
        _context.Add(post);
    }
    public void DeletePost(Post post)
    {
        _context.Posts.Remove(post);
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await _context.Posts.Include(p => p.User).Include(p => p.Comments).ThenInclude(c=>c.User).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Post>> GetPostsAsync(string? sort )
    {
        var query = _context.Posts.Include(p=>p.User).Include(p => p.Comments).AsQueryable();
        query=sort switch
        {
            "fromNewest"=>query.OrderBy(x=>x.Updated),
            "fromLatest"=>query.OrderByDescending(x=>x.Updated),
            "leastLikes"=>query.OrderBy(x=>x.Like),
            "mostLikes"=>query.OrderByDescending(x=>x.Like),
            _=>query.OrderBy(x=>x.Title)
        };
        return await query.ToListAsync();
    }

   
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync()>0;
    }

    
}
