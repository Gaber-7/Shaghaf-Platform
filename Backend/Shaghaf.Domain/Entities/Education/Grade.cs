using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Entities.Courses;

namespace Shaghaf.Domain.Entities.Education;

public class Grade : BaseEntityInt
{
    public int StageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeNumber { get; set; } // 3-12
    public string? Description { get; set; }

    // Navigation properties
    public EducationStage? Stage { get; set; }
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
