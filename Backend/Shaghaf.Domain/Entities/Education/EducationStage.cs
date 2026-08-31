using Shaghaf.Domain.Entities.Base;
using Shaghaf.Domain.Enums;

namespace Shaghaf.Domain.Entities.Education;

public class EducationStage : BaseEntityInt
{
    public string Name { get; set; } = string.Empty;
    public EducationStageType Type { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; }

    // Navigation properties
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}