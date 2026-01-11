using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class StaffWorkSchedule
{
    public int StaffWorkScheduleId { get; set; }

    public int StaffId { get; set; }

    public byte DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Notes { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
