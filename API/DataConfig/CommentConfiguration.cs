using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataConfig;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c=>c.User).WithMany(u=>u.Comments).HasForeignKey(u=>u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        
    }
}
