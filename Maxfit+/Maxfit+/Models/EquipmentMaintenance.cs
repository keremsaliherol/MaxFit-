using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class EquipmentMaintenance
{
    public int MaintenanceId { get; set; }

    public int EquipmentId { get; set; }

    public DateOnly RecordDate { get; set; }

    public string IssueType { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? PlannedFixDate { get; set; }

    public DateOnly? ActualFixDate { get; set; }

    public string? PerformedBy { get; set; }

    public bool IsResolved { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;
}
