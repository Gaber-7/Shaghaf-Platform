using Shaghaf.Domain.Entities.Notifications;

namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Parent entity inheriting from User
/// </summary>
public class Parent : User
{
    public string? Occupation { get; set; }
    public string? Relationship { get; set; }

    // Navigation properties
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();
}
