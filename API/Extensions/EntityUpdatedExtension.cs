using System;
using API.Entities;

namespace API.Extensions;

public static class EntityUpdatedExtension
{
    public static void AddEntityUpdateddInfo(this BaseEntity entity)
    {
        entity.Updated = DateTimeOffset.UtcNow;
    }
}
