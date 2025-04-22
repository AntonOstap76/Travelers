using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataConfig;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
       builder.HasOne(p=>p.User).WithMany(u=>u.Posts).HasForeignKey(p=>p.UserId).IsRequired();

       builder.OwnsOne(p=>p.Location);

       builder.HasMany(p=>p.Comments).WithOne().HasForeignKey(c=>c.PostId).OnDelete(DeleteBehavior.Cascade);
    }
}
