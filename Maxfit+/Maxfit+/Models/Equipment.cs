using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class Equipment
{
    public int EquipmentId { get; set; }

    public string Name { get; set; } = null!;

    public string? Category { get; set; }

    public string? Brand { get; set; }

    public string? Model { get; set; }

    // Models/Equipment.cs dosyasının içine ekle
    public string? Status { get; set; } // Arızalı, Bakımda gibi durumlar için

    public string? SerialNumber { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public bool IsActive { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<EquipmentMaintenance> EquipmentMaintenances { get; set; } = new List<EquipmentMaintenance>();

    public virtual ICollection<EquipmentOrder> EquipmentOrders { get; set; } = new List<EquipmentOrder>();
}
