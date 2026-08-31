namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Admin entity inheriting from User
/// </summary>
public class Admin : User
{
    public string? Department { get; set; }
    public List<string> Permissions { get; set; } = new List<string>();
    public DateTime? AssignedDate { get; set; }
}
