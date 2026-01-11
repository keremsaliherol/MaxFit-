using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class EquipmentOrder
{
    public int EquipmentOrderId { get; set; }

    public int EquipmentId { get; set; }

    public string? SupplierName { get; set; }

    public int Quantity { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? ExpectedDeliveryDate { get; set; }

    public DateOnly? ReceivedDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Equipment Equipment { get; set; } = null!;
}
