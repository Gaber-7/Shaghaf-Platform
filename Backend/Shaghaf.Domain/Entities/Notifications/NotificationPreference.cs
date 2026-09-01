using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Users;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Notifications;

public class NotificationPreference : BaseEntity
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationChannel Channel { get; set; }
    public bool IsEnabled { get; set; } = true;

    // Navigation properties
    public User? User { get; set; }
}
