using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class CourseSessionPhoto
{
    public int PhotoId { get; set; }

    public int CourseSessionId { get; set; }

    public string FilePath { get; set; } = null!;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }

    public int? UploadedByUserId { get; set; }

    public virtual CourseSession CourseSession { get; set; } = null!;

    public virtual User? UploadedByUser { get; set; }
}
