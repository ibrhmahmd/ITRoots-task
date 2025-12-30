using System;

namespace StudentRegistrationSystem.Domain.Entities;

public abstract class BaseEntity
{
    public string Id { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
