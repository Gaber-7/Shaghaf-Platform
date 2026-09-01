using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;

namespace Shaghaf.Domain.Entities.Education;

public class Subject : BaseEntityInt
{
    public int StageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public int Order { get; set; }

    // Navigation properties
    public EducationStage? Stage { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
