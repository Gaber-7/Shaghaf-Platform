namespace Shaghaf.Domain.Entities.Users;

/// <summary>
/// Parent entity
/// </summary>
public class Parent : User
{
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    
    // Navigation properties
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
