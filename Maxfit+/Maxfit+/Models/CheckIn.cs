using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class CheckIn
{
    public int CheckInId { get; set; }

    public int MemberId { get; set; }

    public DateTime CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? Source { get; set; }

    public virtual Member Member { get; set; } = null!;
}
