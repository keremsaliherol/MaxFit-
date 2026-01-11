using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class CourseSession
{
    public int CourseSessionId { get; set; }
    public int ClassId { get; set; }
    public int RoomId { get; set; }
    public int TrainerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }
    public bool IsCanceled { get; set; }
    public string? Notes { get; set; }

    // Yanlarına ? ekleyerek nullable yaptık
    public virtual Class? Class { get; set; }
    public virtual Room? Room { get; set; }
    public virtual Staff? Trainer { get; set; }

    public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();
    public virtual ICollection<CourseSessionPhoto> CourseSessionPhotos { get; set; } = new List<CourseSessionPhoto>();
}