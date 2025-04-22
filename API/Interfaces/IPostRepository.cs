using System;
using API.Entities;

namespace API.Interfaces;

public interface IPostRepository
{
    
    void AddPost(Post post);
    void DeletePost(Post post);
    Task<Post?> GetByIdAsync(int id);
    Task<bool> SaveChangesAsync();

    //TODO: add page spec
    Task<IReadOnlyList<Post>>GetPostsAsync();


}
