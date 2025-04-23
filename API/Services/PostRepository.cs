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

    public async Task<IReadOnlyList<Post>> GetPostsAsync()
    {
        return await _context.Posts.Include(p=>p.User).Include(p => p.Comments).OrderByDescending(p=>p.Created).ToListAsync();
    }
    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync()>0;
    }

    public void UpdatePost(Post post)
    {
        _context.Entry(post).State=EntityState.Modified;
    }
}
