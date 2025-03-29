using System;
using System.Text.RegularExpressions;
using API.Entities;
using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class StoreContext(DbContextOptions options) : IdentityDbContext<User>(options)
{

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    
        base.OnModelCreating(modelBuilder);
    }


}
