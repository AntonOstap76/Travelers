using System;

namespace API.Entities;

public class BaseEntity
{
    public int Id { get; set; }

    public DateTimeOffset Updated { get; set; }
    public DateTimeOffset Created { get; set; }
}
