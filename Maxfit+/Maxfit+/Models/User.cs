using System;
using System.Collections.Generic;

namespace Maxfit_.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Email { get; set; }

    public string FullName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<CourseSessionPhoto> CourseSessionPhotos { get; set; } = new List<CourseSessionPhoto>();

    public virtual ICollection<MemberNotification> MemberNotifications { get; set; } = new List<MemberNotification>();

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
