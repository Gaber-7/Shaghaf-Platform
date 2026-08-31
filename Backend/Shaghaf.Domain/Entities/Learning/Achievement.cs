using Shaghaf.Domain.Entities.Base;

namespace Shaghaf.Domain.Entities.Learning;

public class Achievement : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public BadgeType Type { get; set; }
    public int PointsReward { get; set; }
    public Rarity Rarity { get; set; }

    // Navigation properties
    public ICollection<StudentAchievement> StudentAchievements { get; set; } = new List<StudentAchievement>();
}

public enum BadgeType
{
    Milestone = 1,
    Streak = 2,
    Performance = 3,
    Participation = 4
}

public enum Rarity
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}