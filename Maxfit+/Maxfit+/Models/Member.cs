using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; // ForeignKey için gerekli

namespace Maxfit_.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateTime RegisterDate { get; set; }

    public bool IsActive { get; set; }

    public string? PhotoUrl { get; set; } // Profile photo path


    // --- EKLEMEN GEREKEN ALANLAR ---

    // Veritabanındaki Foreign Key sütunu
    public int? MembershipTypeId { get; set; }

    // İlişkili paket nesnesi (Navigation Property)
    [ForeignKey("MembershipTypeId")]
    public virtual MembershipType? MembershipType { get; set; }

    // ------------------------------

    public virtual ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();

    public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();

    public virtual ICollection<MemberNotification> MemberNotifications { get; set; } = new List<MemberNotification>();

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}