using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class CourseRegistration
{
    public int CourseRegistrationId { get; set; }

    public int CourseSessionId { get; set; }

    public int MemberId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual CourseSession CourseSession { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;
}
