using System;
using API.Entities;

namespace API.Extensions;

public static class EntityCreatedExtension
{
    public static void AddEntityCreatedInfo(this BaseEntity entity)
    {
        entity.Created =DateTimeOffset.UtcNow;
    }
}
