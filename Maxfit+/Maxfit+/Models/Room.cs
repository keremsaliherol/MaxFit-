using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();
}
