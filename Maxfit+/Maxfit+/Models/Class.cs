using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DefaultDurationMinutes { get; set; }

    public string GenderRestriction { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();
}
