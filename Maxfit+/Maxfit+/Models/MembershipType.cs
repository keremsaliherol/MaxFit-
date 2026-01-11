using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class MembershipType
{
    public int MembershipTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationInDays { get; set; }

    // Fiyatlandırma için eklenen yeni alan
    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    // Mevcut ilişki (Membership tablosu için)
    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    // Member tablosuyla doğrudan ilişki
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}