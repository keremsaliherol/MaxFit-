using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string Position { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public bool IsActive { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();

    public virtual ICollection<StaffWorkSchedule> StaffWorkSchedules { get; set; } = new List<StaffWorkSchedule>();

    public virtual User? User { get; set; }
}
